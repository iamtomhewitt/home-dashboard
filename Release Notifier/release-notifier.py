import smtplib
import os
import datetime

from email.mime.multipart import MIMEMultipart
from email.mime.text import MIMEText
from email.utils import formatdate

from dotenv import load_dotenv
load_dotenv()

# TODO copy apk file across to dropbox location
# See if we can use dropbox app to create a share link

now = datetime.datetime.now()
my_email = os.environ.get("MY_EMAIL")
my_password = os.environ.get("MY_EMAIL_PASSWORD")

input("Unlock then relock here: https://www.google.com/settings/security/lesssecureapps")

version = input("Dashboard version: ")
send_to = input("Send to (email): ")
dropbox_link = input("Dropbox link: ")

email_text = """Download and install from here:
{}

To install you'll have to browse to the downloaded file on your tablet in file explorer and install it from there.

Whats new: https://github.com/iamtomhewitt/home-dashboard/releases/tag/{}

Tom""".format(dropbox_link, version)

# Construct the email
msg = MIMEMultipart()
msg['From'] = my_email
msg['Date'] = formatdate(localtime=True)
msg['Subject'] = "Home Dashboard " + version
msg.attach(MIMEText(email_text))

# Now send the email
smtp = smtplib.SMTP('smtp.gmail.com')
smtp.starttls()
smtp.login(my_email, my_password)
smtp.sendmail(my_email, send_to, msg.as_string())
smtp.close()

# Log it
print(now.strftime("%Y-%m-%d %H:%M:%S") + " sent email to {}".format(send_to))
