using Microsoft.Owin;
using Owin;
using Swerl.Referee.NerdDinnerSample;

[assembly: OwinStartup(typeof(Startup))]
namespace Swerl.Referee.NerdDinnerSample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
