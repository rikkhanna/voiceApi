using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace voiceApi.VoiceRecognition
{
    public class SpeakerVerificationMethods
    {
        public static Guid SVEnrollSpeaker(SpeakerVerificationService svService)
        {
            // Open the sound file and load it in
            var speakerVerificationEnroll1 = File.OpenRead(Constants.AUDIO_DIRECTORY + "speakerVerificationEnroll1.wav");
            var speakerVerificationEnroll2 = File.OpenRead(Constants.AUDIO_DIRECTORY + "speakerVerificationEnroll2.wav");
            var speakerVerificationEnroll3 = File.OpenRead(Constants.AUDIO_DIRECTORY + "speakerVerificationEnroll3.wav");

            // Create the new profile
            Console.WriteLine("Create new profile for Speaker 1");
            Task<Guid> newProfile = svService.CreateNewProfile("en-us");
            newProfile.Wait();
            Guid id = newProfile.Result;
            Console.WriteLine("Done");
            Console.WriteLine("");

            // Enroll the speaker
            Console.WriteLine("Enroll Speaker: " + id);
            svService.EnrollUser(speakerVerificationEnroll1, id).Wait();
            svService.EnrollUser(speakerVerificationEnroll2, id).Wait();
            svService.EnrollUser(speakerVerificationEnroll3, id).Wait();
            Console.WriteLine("Done");
            Console.WriteLine("");

            return id;
        }

        public static async Task<string> SVVerifySpeaker(SpeakerVerificationService svService)
        {
            // Open the sound file and load it in
            var speaker1Verification = File.OpenRead(Constants.AUDIO_DIRECTORY + "speakerVerification1.wav");

            // Enroll the speaker
            Console.WriteLine("Identify Speaker");
            var obj = await svService.VerifyUser(speaker1Verification);
            return obj;

        }
    }
}
