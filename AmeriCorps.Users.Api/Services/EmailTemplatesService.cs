namespace AmeriCorps.Users.Api.Services;

public interface IEmailTemplatesService
{
    string InviteUserTemplate();
    string SSNNotInFileTemplate();
    string NameDOBGenderNotInFileTemplate();
    string NameDoesNotMathOrOtherTemplate();
}

public class EmailTemplatesService() : IEmailTemplatesService
{
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
                                <p class='center-text'>The success of AmeriCorps largely depends upon an appropriate match between programs and members. Considerable value of your experiences and knowledge you have been invited {1} to create and account and login to ( MAPRAPP ).</p>
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

    public string SSNNotInFileTemplate()
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
                                    <p>The Corporation for National and Community Service was unable to automatically verify the citizenship status of this applicant:</p>
                                    <p>{0}</p>
                                    <p>The individual needs to send a copy of their social security card to the National Service Hotline.</p>
                                <p>To submit documents, contact the National Service Hotline at 1-800-942-2677 or via the web-form found at <a href='https://questions.americorps.gov/app/ask'>https://questions.americorps.gov/app/ask</a> and request a secure transfer link be emailed to you. The email you receive with the link will include additional submission instructions.</p>
                                <p>On the cover letter, please write the subject heading 'Citizenship verification check' and include the person's name and NSPID or NSAID as shown in this email. Please include contact information in case the copies in the fax are not readable.</p>
                            </div>
                        </body>
                        </html>";

            return htmlTemplate;
        }

        public string NameDOBGenderNotInFileTemplate()
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
                                    <p>The Corporation for National and Community Service was unable to automatically verify either the date of birth, gender, or both for this applicant:</p>
                                    <p>{0}</p>
                                    <p>Please have the individual send a copy of their birth certificate and one of the following government authorized documents to the National Service Hotline: passport, driver’s license, military ID, state or federally issued ID, or social security card.</p>
                                <p>To submit documents, contact the National Service Hotline at 1-800-942-2677 or via the web-form found at <a href='https://questions.americorps.gov/app/ask'>https://questions.americorps.gov/app/ask</a> and request a secure transfer link be emailed to you. The email you receive with the link will include additional submission instructions.</p>
                                <p>On the cover letter, please write the subject heading 'Verification check' and include the person's name and NSPID or NSAID as shown in this email. Please include contact information in case the copies in the fax are not readable.</p>
                            </div>
                        </body>
                        </html>";

            return htmlTemplate;
        }
        
        public string NameDoesNotMathOrOtherTemplate()
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
                                    <p>The Corporation for National and Community Service was unable to automatically match the name to the application records for this applicant:</p>
                                    <p>{0}</p>
                                    <p>Please contact the individual and instruct them to send copies of two government authorized documents to the National Service Hotline. One of these documents must be a copy of their social security card and the other can be one of the following: state issued ID, federally issued ID, driver’s license, passport, or birth certificate.</p>
                                <p>To submit documents, contact the National Service Hotline at 1-800-942-2677 or via the web-form found at <a href='https://questions.americorps.gov/app/ask'>https://questions.americorps.gov/app/ask</a> and request a secure transfer link be emailed to you. The email you receive with the link will include additional submission instructions.</p>
                                <p>On the cover letter, please write the subject heading 'Verification check' and include the person's name and NSPID or NSAID as shown in this email. Please include contact information in case the copies in the fax are not readable.</p>
                                <p>If the individual was recently married or divorced and their information is up to date in the My AmeriCorps portal, their name might not be up to date with the Social Security Administration.</p>
                                <p>If this is the case, they will need to contact the Social Security Administration to update their information: https://faq.ssa.gov/link/portal/34011/34019/Article/3749/How-do-I-change-or-correct-my-name-on-my-Social-Security-number-card</p>
                            </div>
                        </body>
                        </html>";

            return htmlTemplate;
        }
}