using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "UIManagerData", menuName = "Managers/UIManagerData")]

public class UIManagerData : ScriptableObject
{
    
    [Header("UI Data")]
    [Tooltip("Fps values")]
    [SerializeField] private List<string> fpsLimit;
    [Header("Resolutions")]
    [Tooltip("Available resolutions")]
    [SerializeField] private List<int> resx;
    [SerializeField] private List<int> resy;
    [SerializeField] private List<string> links;
    [SerializeField] private List<string> notifications;
    [SerializeField] private List<string> notificationType;
    [SerializeField] private List <Sprite> healthImages;
    [SerializeField] private float notificationTime;
    [SerializeField] private float notificationInterval;
    [SerializeField] private float notificationAppearTime;
    [SerializeField] private float notificationDisappearTime;
    [SerializeField] private float startButtonWaitTime;
    [Header("Ability images values")]
    [Range(0f, 1f)]
    [SerializeField] private float appearImageInterval;
    [Range(0f, 1f)]
    [SerializeField] private float appearImageTimeInterval;
    [Range (0f, 1f)]
    [SerializeField] private float disappearImageInterval;
    [Range (0f, 1f)]
    [SerializeField] private float disappearImageTimeInterval;

    public float NotificationDisappearTime => notificationDisappearTime;
    public float NotificationAppearTime => notificationAppearTime;
    public float NotificationInterval => notificationInterval;
    public float NotificationTime => notificationTime;
    public float StartButtonWaitTime => startButtonWaitTime;
    public List<string> NotificationType => notificationType;
    public List<string> Notifications => notifications;
    public List<string> FpsLimit => fpsLimit;
    public List<int> Resx => resx;
    public List <int> Resy => resy;
    public List<string> Links => links;
    public List<Sprite> HealthImages => healthImages;
    public float AppearImageInterval => appearImageInterval;
    public float DisappearImageInterval => disappearImageInterval;
    public float AppearImageTimeInterval => appearImageTimeInterval;
    public float DisappearImageTimeInterval => disappearImageTimeInterval;
}
