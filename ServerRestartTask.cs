using MediaBrowser.Common;
using MediaBrowser.Common.Net;
using MediaBrowser.Common.ScheduledTasks;
using MediaBrowser.Controller;
using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServerRestart
{
    /// <summary>
    /// Downloads trailers from the web at scheduled times
    /// </summary>
    public class ServerRestartTask : IScheduledTask
    {
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>The logger.</value>
        private ILogger Logger { get; set; }
        /// <summary>
        /// Gets or sets the app host
        /// </summary>
        /// <value>The app host.</value>
        private IApplicationHost AppHost { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerRestartTask" /> class.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        /// <param name="host"></param>
        /// <param name="logger">The logger.</param>
        public ServerRestartTask(IApplicationHost host, ILogger logger)
        {
            Logger = logger;
            AppHost = host;
        }

        /// <summary>
        /// Creates the triggers that define when the task will run
        /// </summary>
        /// <returns>IEnumerable{BaseTaskTrigger}.</returns>
        public IEnumerable<ITaskTrigger> GetDefaultTriggers()
        {
            return new ITaskTrigger[]
                {
                    new DailyTrigger { TimeOfDay = TimeSpan.FromHours(3.75) },

                };
        }

        /// <summary>
        /// Returns the task to be executed
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="progress">The progress.</param>
        /// <returns>Task.</returns>
        public async Task Execute(CancellationToken cancellationToken, IProgress<double> progress)
        {
            Logger.Info("Re-starting server as requested/scheduled...");
            progress.Report(100);

            // Don't await this so that we will finish before the restart and not get reported as aborted
            AppHost.Restart();
        }

        /// <summary>
        /// Gets the name of the task
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return "Scheduled Server Restart"; }
        }

        /// <summary>
        /// Gets the category.
        /// </summary>
        /// <value>The category.</value>
        public string Category
        {
            get
            {
                return "Application";
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get { return "Restart the server at a specified time or interval."; }
        }
    }
}
