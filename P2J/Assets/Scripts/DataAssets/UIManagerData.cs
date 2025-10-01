using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UIManagerData", menuName = "Managers/UIManagerData")]

public class UIManagerData : ScriptableObject
{
    
    [Header("UI Data")]
    [SerializeField] private List<string> fpsLimit;
    [SerializeField] private List<int> resx;
    [SerializeField] private List<int> resy;
    [SerializeField] private List<string> links;
    [SerializeField] private List<string> notifications;
    [SerializeField] private List<string> notificationType;
    [SerializeField] private float notificationTime;
    [SerializeField] private float notificationInterval;
    [SerializeField] private float notificationAppearTime;
    [SerializeField] private float notificationDisappearTime;
    [SerializeField] private float startButtonWaitTime;

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
}
