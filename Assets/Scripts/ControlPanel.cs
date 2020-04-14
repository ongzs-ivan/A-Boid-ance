using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControlPanel: MonoBehaviour
{
    [Header("Settings")]
    public Slider minSpeedSlider;
    public TextMeshProUGUI minSpeedText;
    public Slider maxSpeedSlider;
    public TextMeshProUGUI maxSpeedText;
    public Slider alignmentSlider;
    public TextMeshProUGUI alignText;
    public Slider cohesionSlider;
    public TextMeshProUGUI cohesionText;
    public Slider separationSlider;
    public TextMeshProUGUI separationText;
    public Slider targetSlider;
    public TextMeshProUGUI targetText;
    public Slider evasionSlider;
    public TextMeshProUGUI evasionText;

    private float minSpeed = 2;
    private float maxSpeed = 5;
    private float arrivalRadius = 3.0f;
    private float perceptionRadius = 2.5f;
    private float avoidanceRadius = 1;
    private float maxSteerForce = 3;

    private float alignWeight = 1;
    private float cohesionWeight = 1;
    private float separateWeight = 1;
    private float targetWeight = 1;
    
    private float boundsRadius = .27f;
    private float avoidCollisionWeight = 10;
    private float collisionAvoidDst = 5;
    
    private float evadeWeight;
    
    private float fleeRadius;
    private float fleeWeight;
    private float fleeDistance;


    [Space]
    [Header("Settings")]
    public BoidSettings controlsSettings;   // current values
    public BoidSettings initialSettings;    // initial values
    public BoidManager boidManager;

    void Start()
    {
        InitializeSettings();
        ResetSettingsButton();
        InitializePanelValues();
        boidManager.managerSettings = controlsSettings;
    }
    
    public void UpdatePanelValues()
    {
        if (minSpeedSlider.value > maxSpeedSlider.value)
        {
            minSpeedSlider.value = maxSpeedSlider.value;
        }

        minSpeed = minSpeedSlider.value;
        minSpeedText.text = minSpeed.ToString();

        maxSpeed = maxSpeedSlider.value;
        maxSpeedText.text = maxSpeed.ToString();

        alignWeight = alignmentSlider.value;
        alignText.text = alignWeight.ToString();

        cohesionWeight = cohesionSlider.value;
        cohesionText.text = cohesionWeight.ToString();

        separateWeight = separationSlider.value;
        separationText.text = separateWeight.ToString();

        targetWeight = targetSlider.value;
        targetText.text = targetWeight.ToString();

        evadeWeight = evasionSlider.value;
        evasionText.text = evadeWeight.ToString();
    }


    private void InitializePanelValues()
    {
        minSpeedSlider.value = controlsSettings.minSpeed;
        minSpeedText.text = controlsSettings.minSpeed.ToString();

        maxSpeedSlider.value = controlsSettings.maxSpeed;
        maxSpeedText.text = controlsSettings.maxSpeed.ToString();

        alignmentSlider.value = controlsSettings.alignWeight;
        alignText.text = controlsSettings.alignWeight.ToString();

        cohesionSlider.value = controlsSettings.cohesionWeight;
        cohesionText.text = controlsSettings.cohesionWeight.ToString();

        separationSlider.value = controlsSettings.seperateWeight;
        separationText.text = controlsSettings.seperateWeight.ToString();

        targetSlider.value = controlsSettings.targetWeight;
        targetText.text = controlsSettings.targetWeight.ToString();

        evasionSlider.value = controlsSettings.evadeWeight;
        evasionText.text = controlsSettings.evadeWeight.ToString();

    }

    private void InitializeSettings()
    {
        minSpeed = initialSettings.minSpeed;
        maxSpeed = initialSettings.maxSpeed;
        arrivalRadius = initialSettings.arrivalRadius;
        perceptionRadius = initialSettings.perceptionRadius;
        avoidanceRadius = initialSettings.avoidanceRadius;
        maxSteerForce = initialSettings.maxSteerForce;
        alignWeight = initialSettings.alignWeight;
        cohesionWeight = initialSettings.cohesionWeight;
        targetWeight = initialSettings.targetWeight;
        boundsRadius = initialSettings.boundsRadius;
        avoidCollisionWeight = initialSettings.avoidCollisionWeight;
        collisionAvoidDst = initialSettings.collisionAvoidDst;
        evadeWeight = initialSettings.evadeWeight;
        fleeRadius = initialSettings.fleeRadius;
        fleeWeight = initialSettings.fleeWeight;
        fleeDistance = initialSettings.fleeDistance;
    }

    public void ApplyChanges()
    {
        controlsSettings.minSpeed = this.minSpeed;
        controlsSettings.maxSpeed = this.maxSpeed;
        controlsSettings.arrivalRadius = this.arrivalRadius;
        controlsSettings.perceptionRadius = this.perceptionRadius;
        controlsSettings.avoidanceRadius = this.avoidanceRadius;
        controlsSettings.maxSteerForce = this.maxSteerForce;

        controlsSettings.alignWeight = this.alignWeight;
        controlsSettings.cohesionWeight = this.cohesionWeight;
        controlsSettings.seperateWeight = this.separateWeight;
        controlsSettings.targetWeight = this.targetWeight;

        controlsSettings.boundsRadius = this.boundsRadius;
        controlsSettings.avoidCollisionWeight = this.avoidCollisionWeight;
        controlsSettings.collisionAvoidDst = this.collisionAvoidDst;

        controlsSettings.evadeWeight = this.evadeWeight;
        controlsSettings.fleeRadius = this.fleeRadius;
        controlsSettings.fleeWeight = this.fleeWeight;
        controlsSettings.fleeDistance = this.fleeDistance;

        boidManager.UpdateBoidSettings(controlsSettings);
    }



    public void ResetSettingsButton()
    {
        controlsSettings.minSpeed = initialSettings.minSpeed;
        controlsSettings.maxSpeed = initialSettings.maxSpeed;
        controlsSettings.arrivalRadius = initialSettings.arrivalRadius;
        controlsSettings.perceptionRadius = initialSettings.perceptionRadius;
        controlsSettings.avoidanceRadius = initialSettings.avoidanceRadius;
        controlsSettings.maxSteerForce = initialSettings.maxSteerForce;
        controlsSettings.alignWeight = initialSettings.alignWeight;
        controlsSettings.cohesionWeight = initialSettings.cohesionWeight;
        controlsSettings.seperateWeight = initialSettings.seperateWeight;
        controlsSettings.targetWeight = initialSettings.targetWeight;
        controlsSettings.obstacleMask = initialSettings.obstacleMask;
        controlsSettings.boundsRadius = initialSettings.boundsRadius;
        controlsSettings.avoidCollisionWeight = initialSettings.avoidCollisionWeight;
        controlsSettings.collisionAvoidDst = initialSettings.collisionAvoidDst;
        controlsSettings.evadeWeight = initialSettings.evadeWeight;
        controlsSettings.fleeRadius = initialSettings.fleeRadius;
        controlsSettings.fleeWeight = initialSettings.fleeWeight;
        controlsSettings.fleeDistance = initialSettings.fleeDistance;

        InitializeSettings();
        InitializePanelValues();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}