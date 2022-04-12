using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class TrackingTarget : MonoBehaviour
{
    private TrackableBehaviour track;
    public Canvas canvas_target_lost;

    void Start()
    {
        track = this.GetComponent<TrackableBehaviour>();
        if (track)
            track.RegisterOnTrackableStatusChanged(OntrackableStatusChanged);
    }

    void OntrackableStatusChanged(TrackableBehaviour.StatusChangeResult statusChangeResult)
    {
        if (statusChangeResult.NewStatus == TrackableBehaviour.Status.DETECTED ||
            statusChangeResult.NewStatus == TrackableBehaviour.Status.TRACKED ||
            statusChangeResult.NewStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Time.timeScale = 1;
            EnableScript(true);
        } else
        {
            Time.timeScale = 0;
            EnableScript(false);

            GameObject[] respawnParticles = GameObject.FindGameObjectsWithTag("RespawnParticle");
            foreach (var respawnParticle in respawnParticles)
                respawnParticle.SetActive(false);
        }
    }

    void EnableScript(bool enabled)
    {
        Arrow[] arrow_scripts = GameObject.FindObjectsOfType<Arrow>();
        Fireball[] fireBall_scripts = GameObject.FindObjectsOfType<Fireball>();
        WarriorCtrl[] warriorCtrl_scripts = GameObject.FindObjectsOfType<WarriorCtrl>();
        SkeletonCtrl[] skeletonCtrl_scripts = GameObject.FindObjectsOfType<SkeletonCtrl>();
        UnitSpawnController unitSpawnController_script = GameObject.FindObjectOfType<UnitSpawnController>();

        foreach (var script in arrow_scripts)
            script.enabled = enabled;

        foreach (var script in fireBall_scripts)
            script.enabled = enabled;

        foreach (var script in warriorCtrl_scripts)
            script.enabled = enabled;

        foreach (var script in skeletonCtrl_scripts)
            script.enabled = enabled;

        unitSpawnController_script.enabled = enabled;
        canvas_target_lost.enabled = !enabled;
    }
}
