using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

       [HttpGet]
       [Route("verify")]
       public async Task<string> SpeakerVerification()
        {
            var sb = new StringBuilder();
            try
            {
                // Create the service
                Console.WriteLine("Creating the Speaker Verification Service.");
                var svService = new SpeakerVerificationService(Constants.SPEAKER_RECOGNITION_KEY);
                Console.WriteLine("Done");
                Console.WriteLine("");

                // Delete all existing profiles
                Console.WriteLine("Deleting All Existing Profiles.");
                svService.DeleteAllProfiles().Wait();
                Console.WriteLine("Done");
                Console.WriteLine("");

                // Enroll Speaker 
                Guid id = SpeakerVerificationMethods.SVEnrollSpeaker(svService);

                // Verify Speaker
                string obj = await SpeakerVerificationMethods.SVVerifySpeaker(svService);
                sb.AppendLine($"Profile Id: {id}, obj: {obj}");
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