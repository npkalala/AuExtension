using CoreAudioApi;
using System;

namespace AuExtension.Extend.VoiceControl
{
    public static class SystemVoice
    {
        private delegate void UpdateSliderDelegate(object obj);

        private static MMDevice m_device;

        //private bool m_bUpdate = true;
        private static MMDeviceEnumerator devEnum;

        static SystemVoice()
        {
            AuInit();
        }

        public static void AuInit()
        {
            devEnum = new MMDeviceEnumerator();
            m_device = devEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);
        }

        public static int GetVolume()
        {
            decimal d = decimal.Parse((m_device.AudioEndpointVolume.MasterVolumeLevelScalar * 100).ToString());
            return (int)Math.Round(d, 0, MidpointRounding.AwayFromZero);
            //return (int)(m_device.AudioEndpointVolume.MasterVolumeLevelScalar * 100);
        }
        /// <summary>
        /// The Range of Volume is 0-100
        /// </summary>
        /// <param name="Volume"></param>
        public static void SetVolume(float Volume)
        {
            if (Volume > 1f)
                Volume = 1f;
            if (Volume < 0)
                Volume = 0;
            m_device.AudioEndpointVolume.MasterVolumeLevelScalar = Volume;// (Volume / 10.0f);
        }
    }
}
