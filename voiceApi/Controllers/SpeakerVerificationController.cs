﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using voiceApi.BaseClasses;
using voiceApi.VoiceRecognition;

namespace voiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpeakerVerificationController : Controller
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Speaker Verification API" };
        }

       

        [HttpPost]
        [Route("enroll")]
        public string EnrollSpeaker([FromBody] EnrollSpeaker enrollSpeaker )
        {
            var sb = new StringBuilder();

            // Create the service
            Console.WriteLine("Creating the Speaker Verification Service.");
            var svService = new SpeakerVerificationService(Constants.SPEAKER_RECOGNITION_KEY);
            Console.WriteLine("Done");
            Console.WriteLine("");

            // Enroll Speaker 
            Guid id = SpeakerVerificationMethods.SVEnrollSpeaker(svService, enrollSpeaker);
            sb.Append($"Profile_id: {id}");
            return sb.ToString();
        }



        [HttpPost]
       [Route("verify")]
       public async Task<string> SpeakerVerification([FromBody] VerifySpeaker verifySpeaker)
        {
            var sb = new StringBuilder();
            try
            {
                // Create the service
                Console.WriteLine("Creating the Speaker Verification Service.");
                var svService = new SpeakerVerificationService(Constants.SPEAKER_RECOGNITION_KEY);
                Console.WriteLine("Done");
                Console.WriteLine("");

                //Delete all existing profiles
                //Console.WriteLine("Deleting All Existing Profiles.");
                //svService.DeleteAllProfiles().Wait();
                //Console.WriteLine("Done");
                //Console.WriteLine("");



                // Verify Speaker
                string obj = await SpeakerVerificationMethods.SVVerifySpeaker(svService, verifySpeaker);
                sb.AppendLine($"obj: {obj}");
                return sb.ToString();

                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return ex.Message.ToString();
            }
            
        }
    }
}