using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalQueue : MonoBehaviour
{
    private struct DecalData
    {
        public Vector3 position;
        public Quaternion rotation;
        public float size;
        public Color color;
    }

    [SerializeField]
    private int MaxDecals = 16;
    private ParticleSystem System;
    private ParticleSystem.Particle[] Particles;
    private DecalData[] Decals;
    private int DecalIndex = 0;
    [SerializeField]
    private float MinDecalSize = 0.1f;
    [SerializeField]
    private float MaxDecalSize = 0.5f;
    [SerializeField]
    private float SurfaceOffset = 0.01f;
    [SerializeField]
    private Color DecalColor = Color.red;
    private void Awake()
    {
        Decals = new DecalData[MaxDecals];
        Particles = new ParticleSystem.Particle[MaxDecals];
        System = GetComponent<ParticleSystem>();
        for (int i = 0; i < Decals.Length; i++)
        {
            Decals[i] = new DecalData();
        }
    }

    public void OnShotHit(RaycastHit hit)
    {
        SetParticleData(hit);
        DisplayParticles();
    }

    private void SetParticleData(RaycastHit hit)
    {
        DecalIndex = DecalIndex % Decals.Length;
        Decals[DecalIndex].position = hit.point + hit.normal * SurfaceOffset;
        Decals[DecalIndex].rotation = Quaternion.LookRotation(hit.normal);
        Decals[DecalIndex].rotation *= Quaternion.Euler(0, 0, Random.Range(0, 360));
        Decals[DecalIndex].size = Random.Range(MinDecalSize, MaxDecalSize);
        DecalIndex++;
    }

    private void DisplayParticles()
    {
        for (int i = 0; i < Decals.Length; i++)
        {
            Particles[i].position = Decals[i].position;
            Particles[i].rotation3D = Decals[i].rotation.eulerAngles;
            Particles[i].startSize = Decals[i].size;
            Particles[i].startColor = DecalColor;
        }
        System.SetParticles(Particles, Particles.Length);
    }
}
