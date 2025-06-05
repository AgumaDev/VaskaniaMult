using UnityEngine;
public class SoundFadeController : MonoBehaviour
{
    public AudioReverbFilter audioReverbFilter;
    public enum ReverbZone
    {
        Lobby,
        Outside,
        Church,
        Basement,
        Backrooms
    }
    
    private void Start()
    {
        audioReverbFilter = GetComponentInChildren<AudioReverbFilter>();
    }
    private void ApplyReverb(ReverbZone zoneReverb)
    {
        switch (zoneReverb)
        {
            case ReverbZone.Lobby:
                audioReverbFilter.reverbPreset = AudioReverbPreset.Room;
                break;
            case ReverbZone.Outside:
                audioReverbFilter.reverbPreset = AudioReverbPreset.Plain;
                break;
            case ReverbZone.Church:
                audioReverbFilter.reverbPreset = AudioReverbPreset.Auditorium;
                break;
            case ReverbZone.Basement:
                audioReverbFilter.reverbPreset = AudioReverbPreset.Cave;
                break;
            case ReverbZone.Backrooms:
                audioReverbFilter.reverbPreset = AudioReverbPreset.ParkingLot;
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        var zoneTrigger = other.GetComponent<ReverbZoneTrigger>();
        if (zoneTrigger == null) return;

        ApplyReverb(zoneTrigger.zoneType);
    }
    private void OnTriggerStay(Collider other)
    {
        var zoneTrigger = other.GetComponent<ReverbZoneTrigger>();
        if (zoneTrigger == null) return;

        ApplyReverb(zoneTrigger.zoneType);    
    }
    private void OnTriggerExit(Collider other)
    { 
        var zoneTrigger = other.GetComponent<ReverbZoneTrigger>();
        if (zoneTrigger == null) return;

        audioReverbFilter.reverbPreset = AudioReverbPreset.Off;   
    }
}
