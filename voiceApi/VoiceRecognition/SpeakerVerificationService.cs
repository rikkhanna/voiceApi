using Microsoft.ProjectOxford.SpeakerRecognition;
using Microsoft.ProjectOxford.SpeakerRecognition.Contract;
using Microsoft.ProjectOxford.SpeakerRecognition.Contract.Verification;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
namespace voiceApi.VoiceRecognition
{
        public class SpeakerVerificationService
        {
            public SpeakerVerificationServiceClient serviceClient { get; }

            public SpeakerVerificationService(string subscriptionKey)
            {
                serviceClient = new SpeakerVerificationServiceClient(subscriptionKey);
            }

            public async Task DeleteAllProfiles()
            {
                Task<Profile[]> profiles = serviceClient.GetProfilesAsync();
                profiles.Wait();

                foreach (var item in profiles.Result)
                {
                    Console.WriteLine("Deleting Profile Id: " + item.ProfileId);
                    await serviceClient.DeleteProfileAsync(item.ProfileId);
                }
            }
            public async Task<Guid> CreateNewProfile(string locale)
            {
                try
                {
                    CreateProfileResponse creationResponse = await serviceClient.CreateProfileAsync(locale);
                    return creationResponse.ProfileId;
                }
                catch (CreateProfileException ex)
                {
                    Console.WriteLine("Profile Creation Error: " + ex.Message);
                    throw;
                }
                catch (GetProfileException ex)
                {
                    Console.WriteLine("Error Retrieving The Profile: " + ex.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    throw;
                }
            }

            public async Task EnrollUser(Stream audioStream, Guid profileId)
            {
                try
                {
                    Enrollment response = await serviceClient.EnrollAsync(audioStream, profileId);

                    if (response.RemainingEnrollments == 0)
                    {
                        Console.WriteLine("Enrolled");
                    }
                    else
                    {
                        Console.WriteLine("Remaining Number of Enrollments: " + response.RemainingEnrollments.ToString());
                    }
                }
                catch (EnrollmentException ex)
                {
                    Console.WriteLine("Enrollment Error: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            public async Task<string> VerifyUser(Stream audioStream)
            {
                var sb = new StringBuilder();
            try
                {
                    Console.WriteLine("Verifying...");
                    Profile[] selectedProfiles = await serviceClient.GetProfilesAsync();
                var profileId = "";
                var result = "";
                var confidenceLevel = "";
                    for (int i = 0; i < selectedProfiles.Length; i++)
                    {
                        Verification response = await serviceClient.VerifyAsync(audioStream, selectedProfiles[i].ProfileId);
                    profileId = selectedProfiles[i].ProfileId.ToString();
                    result = response.Result.ToString();
                    confidenceLevel = response.Confidence.ToString();
                        Console.WriteLine("ProfileId: " + selectedProfiles[i].ProfileId.ToString());
                        Console.WriteLine("Result: " + response.Result.ToString());
                        Console.WriteLine("Confidence Level: " + response.Confidence.ToString());
                    }
                    Console.WriteLine("Verification Complete");
                sb.AppendLine($"ProfileID: {profileId}, Result: {result}, Confidence: {confidenceLevel}");
                return sb.ToString();
                }
                catch (VerificationException ex)
                {
                    Console.WriteLine("Speaker Verification Error: " + ex.Message);
                return ex.Message;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                return ex.Message;
            }
            }
        }
}