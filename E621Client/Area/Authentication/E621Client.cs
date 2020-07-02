﻿using System.Threading.Tasks;

namespace Noppes.E621
{
    public partial class E621Client
    {
        /// <summary>
        /// Whether or not a user is currently logged in.
        /// </summary>
        public bool HasLogin => Credentials != null;

        /// <summary>
        /// Credentials used to authenticate requests with.
        /// </summary>
        internal E621Credentials? Credentials { get; set; }

        /// <summary>
        /// Logs the user in based on their username and API key. It will validate the provided credentials and return
        /// whether or not the log in was successful or not.
        /// </summary>
        /// <param name="username">Username used for logging in.</param>
        /// <param name="apiKey">API key used for logging in.</param>
        public async Task<bool> LogInAsync(string username, string apiKey)
        {
            if (HasLogin)
                throw E621ClientAlreadyLoggedInException.Create();

            Credentials = new E621Credentials(username, apiKey);

            var success = true;
            try
            {
                await GetOwnFavoritesAsync().ConfigureAwait(false);
            }
            catch (E621ClientUnauthorizedException)
            {
                success = false;
            }

            if (success)
                return success;

            Credentials = null;
            return success;
        }

        /// <summary>
        /// Logs a user out.
        /// </summary>
        public void Logout() => Credentials = null;
    }
}
