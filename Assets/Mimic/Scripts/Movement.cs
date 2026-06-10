using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace MimicSpace
{
    public class Movement : MonoBehaviour
    {
        public enum MimicState
        {
            Wandering,
            Chasing,
            Searching
        }

        [Header("References")]
        public Transform player;

        [Header("Movement")]
        public float wanderRadius = 8f;
        public float wanderSpeed = 2.5f;
        public float chaseSpeed = 9f;

        [Header("Hearing")]
        public float hearingRange = 14f;
        public float playerMoveThreshold = 0.1f;
        public float loseInterestTime = 3f;

        [Header("Catch Player")]
        public float catchDistance = 2f;

        private Mimic myMimic;
        private NavMeshAgent agent;

        private MimicState currentState = MimicState.Wandering;

        private Vector3 lastPlayerPosition;
        private Vector3 lastHeardPosition;

        private float loseInterestTimer;

        void Start()
        {
            myMimic = GetComponent<Mimic>();
            agent = GetComponent<NavMeshAgent>();

            if (player == null)
            {
                GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

                if (playerObject != null)
                    player = playerObject.transform;
            }

            if (player != null)
                lastPlayerPosition = player.position;

            agent.speed = wanderSpeed;
            PickWanderDestination();
        }

        void Update()
        {
            if (player == null || agent == null || myMimic == null)
                return;

            if (CanHearPlayer())
            {
                currentState = MimicState.Chasing;
                lastHeardPosition = player.position;
                loseInterestTimer = loseInterestTime;

                agent.speed = chaseSpeed;
                agent.SetDestination(lastHeardPosition);
            }

            switch (currentState)
            {
                case MimicState.Wandering:
                    UpdateWandering();
                    break;

                case MimicState.Chasing:
                    UpdateChasing();
                    break;

                case MimicState.Searching:
                    UpdateSearching();
                    break;
            }

            myMimic.velocity = agent.velocity;
            lastPlayerPosition = player.position;
        }

        bool CanHearPlayer()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer > hearingRange)
                return false;

            float playerSpeed = Vector3.Distance(player.position, lastPlayerPosition) / Time.deltaTime;

            return playerSpeed > playerMoveThreshold;
        }

        void UpdateWandering()
        {
            agent.speed = wanderSpeed;

            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                PickWanderDestination();
            }
        }

        void UpdateChasing()
        {
            agent.speed = chaseSpeed;

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= catchDistance)
            {
                Debug.Log("Mimic caught the player!");
                RestartCurrentScene();
                return;
            }

            loseInterestTimer -= Time.deltaTime;

            if (loseInterestTimer <= 0f)
            {
                currentState = MimicState.Searching;
                agent.SetDestination(lastHeardPosition);
            }
        }

        void UpdateSearching()
        {
            agent.speed = wanderSpeed;

            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                currentState = MimicState.Wandering;
                PickWanderDestination();
            }
        }

        void PickWanderDestination()
        {
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection += transform.position;

            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }

        void RestartCurrentScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}