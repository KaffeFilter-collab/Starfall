using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

// Discord_RP Script by Devid Dondorf and Magnus Cochems
// Put this script into the Scene. If you Play the Scene Discord will start displaying it!
//You can apply this script as a Component to a GameManager or a Player


namespace Discord_RP
{
    public class RichPresenceManager : MonoBehaviour
    {
        private Discord_RP. _discord;
        private long unixTimestamp;

        private void Awake() {
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            // Call the method to get the Unix timestamp
            unixTimestamp = GetUnixTimestamp();

            //Data With APP ID
			// You get it from the Discord (https://discord.com/developers)
            _discord = new Discord.Discord(ADD_APPLICATION_ID_HERE, (ulong)Discord.CreateFlags.NoRequireDiscord);
            ChangeActivity();
        }

        long GetUnixTimestamp()
        {
            // Get the current time in UTC
            DateTime currentTime = DateTime.UtcNow;

            // Define the Unix epoch (January 1, 1970, at midnight)
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // Calculate the Unix timestamp
            long unixTimestamp = (long)(currentTime - unixEpoch).TotalSeconds;

            return unixTimestamp;
        }

        private void Update()
        {
            _discord.RunCallbacks();
        }

        private void OnDisable()
        {
            _discord.Dispose();
        }

        private void ChangeActivity()
        {
            var activityManager = _discord.GetActivityManager();
            var activity = new Discord.Activity
            {
                State = "Playing",
                Details = "Playing",
                Assets = {
					// Place The image that should be displayed in Discord here!
                    LargeImage = "PLACE_IMAGE_HERE"
                },
                Timestamps = {
                    Start = 0 + unixTimestamp
                },
            };
            activityManager.UpdateActivity(activity, (res) => { });
        }
    }
}
