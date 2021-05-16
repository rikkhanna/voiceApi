using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using voiceApi.BaseClasses;

namespace voiceApi.VoiceRecognition
{
    public class SpeakerVerificationMethods
    {
        public static Guid SVEnrollSpeaker(SpeakerVerificationService svService, EnrollSpeaker enrollSpeaker)
        {
            // Open the sound file and load it in
            var speakerVerificationEnroll1 = File.OpenRead(enrollSpeaker.enrollFile1);
            var speakerVerificationEnroll2 = File.OpenRead(enrollSpeaker.enrollFile2);
            var speakerVerificationEnroll3 = File.OpenRead(enrollSpeaker.enrollFile3);

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



        public static async Task<string> SVVerifySpeaker(SpeakerVerificationService svService, VerifySpeaker verifySpeaker)
        {
            // Open the sound file and load it in
            var speaker1Verification = File.OpenRead(verifySpeaker.file);

            // Enroll the speaker
            Console.WriteLine("Identify Speaker");
            var obj = await svService.VerifyUser(speaker1Verification);
            return obj;

        }
    }
}
