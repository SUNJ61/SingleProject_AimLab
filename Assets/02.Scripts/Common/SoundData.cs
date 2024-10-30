using UnityEngine;

public class SoundData
{ //������Ʈ�� ���ֽ�Ű�� ���� �ڽ� �����͸� �����Ѵ�.
    public GameObject SoundBox;
    public AudioSource SoundBox_AudioSource;

    public float? PlayLength;

    public SoundData (GameObject soundBox, AudioSource soundBox_AudioSource, float? playLength)
    {
        SoundBox = soundBox;
        SoundBox_AudioSource = soundBox_AudioSource;
        PlayLength = playLength;
    }
}
