using Microsoft.Extensions.Logging;

namespace AmeriCorps.Users.Api.Services
{

    public interface IEmailTemplates
    {
        string InviteUserTemplate();
    }
    public class EmailTemplates(ILogger<EmailTemplates> logger
                           ) : IEmailTemplates
    {
        private readonly ILogger<EmailTemplates> _logger = logger;

        public string InviteUserTemplate()
        {
            string htmlTemplate = @"
                        <!DOCTYPE html>
                        <html lang='en'>
                        <head>
                            <meta charset='UTF-8'>
                            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                            <style>
                                body {{
                                    font-family: Arial, sans-serif;
                                    background-color: #2d3e50;
                                    margin: 0;
                                    padding: 10px 0;
                                    display: flex;
                                    justify-content: center;
                                    align-items: center;
                                    min-height: 100vh;
                                }}
                                .email-wrapper {{
                                    background-color: #ffffff;
                                    padding: 40px;
                                    border-radius: 0; /* Removed border radius */
                                    width: 80%;
                                    max-width: 600px;
                                    margin: 0 10%;
                                    text-align: center;
                                    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                                }}
                                .email-content {{
                                    text-align: justify;
                                }}
                                .email-content p {{
                                    line-height: 1.6;
                                    margin-bottom: 20px;
                                    font-size: 14px; /* Adjusted font size */
                                    color: #333333; /* Adjusted font color */
                                    font-weight: bold; /* Make content bold */
                                }}
                                .email-content .recipient-name {{
                                    font-weight: bold;
                                    display: block;
                                    text-align: center;
                                    font-size: 18px; /* Increased font size */
                                    color: #000000; /* Black color */
                                    margin-top: -10px; /* Reduced space between Dear and recipient name */
                                }}
                                .email-button {{
                                    display: inline-block;
                                    padding: 12px 24px; /* Increased padding for the button */
                                    color: #ffffff;
                                    background-color: #007bff;
                                    border-radius: 25px;
                                    text-decoration: none;
                                    font-size: 18px; /* Increased font size */
                                    margin: 5px auto; /* Reduced space around button */
                                    font-weight: bold;
                                }}
                                .greeting {{
                                    font-size: 18px; /* Increased font size */
                                    color: #333333;
                                    font-weight: bold;
                                    text-align: center; /* Center align the greeting */
                                }}
                                .note {{
                                    font-size: 14px; /* Increased font size */
                                    color: #777777;
                                    font-weight: bold;
                                    text-align: center; /* Center align the note */
                                }}
                                .center-text {{
                                    text-align: center;
                                }}
                            </style>
                        </head>
                        <body>
                            <div class='email-wrapper'>
                                <div class='email-content'>
                                    <p class='greeting'>Dear</p>
                                    <p class='recipient-name'>{0}</p>
                                    <p>AmeriCorps engages more than 70,000 Americans each year in results-driven service opportunities sponsored by thousands of local and national non-profits, public agencies, and faith-based community organizations.</p>
                                    <p class='center-text'>The success of AmeriCorps largely depends upon an appropriate match between programs and members. Considerable value of your experiences and knowledge you have been invited by {1} to create and account and login to ( MAPRAPP ).</p>
                                    <p class='center-text'>Please click on the link below to create an account and login to the MAPR APP. SIGN IN WITH ID.me LOGIN.GOV</p>
                                    <div style='text-align: center;'>
                                        <a href='{2}' class='email-button'>Click Here</a>
                                    </div>
                                    <p class='center-text'>Please do not reply to this message. If you have any questions or need further assistance please call <span class='center-text'>1-800-942-2677.</span></p>
                                    <p class='note'>***PLEASE DO NOT REPLY TO THIS MESSAGE***</p>
                                </div>
                            </div>
                        </body>
                        </html>";
            
            return htmlTemplate; 
        }

      
    }
}
