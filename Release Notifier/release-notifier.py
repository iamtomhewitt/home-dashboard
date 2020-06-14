import smtplib
import os
import datetime
import dropbox

from email.mime.multipart import MIMEMultipart
from email.mime.text import MIMEText
from email.utils import formatdate

from dotenv import load_dotenv
load_dotenv()

input("\nUnlock then relock here: https://www.google.com/settings/security/lesssecureapps \n")

my_email = os.environ.get("MY_EMAIL")
my_password = os.environ.get("MY_EMAIL_PASSWORD")
dropbox_api_key = os.environ.get("DROPBOX_API_KEY")
dbx = dropbox.Dropbox(dropbox_api_key)

dashboard_name = input("Dashboard file name (with extension): ")
version = input("Dashboard version: ")
send_to = input("Send to (email): ")

# Upload to dropbox
with open("../Builds/{}".format(dashboard_name), 'rb') as f:
    print("\nUploading to dropbox...")
    dbx.files_upload(f.read(), "/Home Dashboards/{}".format(dashboard_name))
    print("Done!")
    link = dbx.sharing_create_shared_link(path="/Home Dashboards/{}".format(dashboard_name)).url

# Construct the email
email_text = """Download and install from here:\n{}\n\nTo install you'll have to browse to the downloaded file on your tablet in file explorer and install it from there.\n\nWhats new: https://github.com/iamtomhewitt/home-dashboard/blob/master/CHANGELOG.md\n\nTom""".format(link)

msg = MIMEMultipart()
msg['From'] = my_email
msg['Date'] = formatdate(localtime=True)
msg['Subject'] = "Home Dashboard " + version
msg.attach(MIMEText(email_text))

# Now send the email
print("\nSending email to {}".format(send_to))
smtp = smtplib.SMTP('smtp.gmail.com')
smtp.starttls()
smtp.login(my_email, my_password)
smtp.sendmail(my_email, send_to, msg.as_string())
smtp.close()

# Log it
print ("Sent email to {} at {}".format(send_to,  datetime.datetime.now()))