using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(LineRenderer))]
public class beamcast : MonoBehaviour {

    public GameObject rayCaster;
    public GameObject explosionPrefab;

    float beamTimer = 0;

    LineRenderer line;

    ParticleSystem particles;

	void Start () {
        line = GetComponent<LineRenderer>();
        particles = GetComponent<ParticleSystem>();
        line.SetVertexCount(2);
        line.enabled = false;
        setParticleEmission(true);
    }

    // Update is called once per frame
    void Update () {
        if (rayCaster == null)
            rayCaster = gameObject;

        Vector3 rayPosition = rayCaster.transform.position;
        Vector3 rayVector = rayCaster.transform.forward;

        if (Input.GetMouseButton(0))
        {
            beamTimer += Time.deltaTime;
            if (beamTimer < 2)
            {
                setParticleEmission(true);
                particles.startSize = 1f;

                ParticleSystem.ShapeModule shape = particles.shape;
                shape.shapeType = ParticleSystemShapeType.Sphere;
                shape.radius = 0.5f;
                particles.startSpeed = -0.5f;

            }
            else if (beamTimer < 2)
            {
                line.SetPosition(0, rayPosition);
                line.SetPosition(1, rayPosition + rayVector * 100);
                line.SetWidth(0.5f * beamTimer, 0.5f * beamTimer);
                line.enabled = true;
                ParticleSystem.ShapeModule shape = particles.shape;
                shape.shapeType = ParticleSystemShapeType.Cone;
                shape.angle = 50;
                shape.radius = 0.1f;
                particles.startSpeed = 5f;
                ParticleSystem.EmissionModule emission = particles.emission;
                emission.rate = 10;
            }
            else if (beamTimer < 6)
            {
                line.SetPosition(0, rayPosition);
                line.SetPosition(1, rayPosition + rayVector * 100);
                line.SetWidth(0.5f * beamTimer, 0.5f * beamTimer);
                line.enabled = true;
                ParticleSystem.EmissionModule emission = particles.emission;
                emission.rate = 10;

                RaycastHit hit;

                if (Physics.Raycast(new Ray(rayPosition, rayVector), out hit))
                {
                    Instantiate(explosionPrefab, hit.point, Quaternion.identity);
                    beamTimer = 6;
                }
            }
            else if (beamTimer < 8)
            {
                line.enabled = false;
                setParticleEmission(false);
            }
            else
            {
                beamTimer = 0f;
            }
        }
        else
        {
            beamTimer = 0f;
            line.enabled = false;
            setParticleEmission(false);
        }
	}

    private void setParticleEmission(bool enabled)
    {
        ParticleSystem.EmissionModule emission = particles.emission;
        emission.enabled = enabled;
    }

}
