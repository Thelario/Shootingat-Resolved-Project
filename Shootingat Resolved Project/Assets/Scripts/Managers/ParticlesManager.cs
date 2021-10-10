using UnityEngine;

namespace PabloLario.Managers
{
#pragma warning disable CS0618 // El tipo o el miembro están obsoletos

    public class ParticlesManager : Singleton<ParticlesManager>
    {
        public GameObject GetParticles(ParticleType pt)
        {
            foreach (Particle p in Assets.Instance.particlesArray)
            {
                if (p.type == pt)
                    return p.particlePrefab;
            }

            Debug.LogError("Particle Prefab Not Found");
            return null;
        }

        public void CreateParticle(ParticleType pt, Vector3 pos)
        {
            Instantiate(GetParticles(pt), pos, Quaternion.identity);
        }

        public void CreateParticle(ParticleType pt, Vector3 pos, Quaternion rotation)
        {
            Instantiate(GetParticles(pt), pos, rotation);
        }

        public void CreateParticle(ParticleType pt, Vector3 pos, Quaternion rotation, Transform parent)
        {
            Instantiate(GetParticles(pt), pos, rotation, parent);
        }

        public void CreateParticle(ParticleType pt, Vector3 pos, float liveTime)
        {
            Destroy(Instantiate(GetParticles(pt), pos, Quaternion.identity), liveTime);
        }

        public void CreateParticle(ParticleType pt, Vector3 pos, float liveTime, Quaternion rotation)
        {
            Destroy(Instantiate(GetParticles(pt), pos, rotation), liveTime);
        }

        public void CreateParticle(ParticleType pt, Vector3 pos, float liveTime, Quaternion rotation, Transform parent)
        {
            Destroy(Instantiate(GetParticles(pt), pos, rotation, parent), liveTime);
        }

        public void CreateParticle(ParticleType pt, Vector3 pos, float liveTime, Color color)
        {
            GameObject go = Instantiate(GetParticles(pt), pos, Quaternion.identity);
            go.GetComponent<ParticleSystem>().startColor = color;
            Destroy(go, liveTime);
        }

        public void CreateParticle(ParticleType pt, Vector3 pos, float liveTime, Quaternion rotation, Color color)
        {
            GameObject go = Instantiate(GetParticles(pt), pos, rotation);
            go.GetComponent<ParticleSystem>().startColor = color;
            Destroy(go, liveTime);
        }

        public void CreateParticle(ParticleType pt, Vector3 pos, float liveTime, Quaternion rotation, Transform parent, Color color)
        {
            GameObject go = Instantiate(GetParticles(pt), pos, rotation, parent);
            go.GetComponent<ParticleSystem>().startColor = color;
            Destroy(go, liveTime);
        }
    }
}