using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using ramp_festivalService.DataObjects;
using ramp_festivalService.Models;

namespace ramp_festivalService.Controllers
{
    public class TodoItemController : TableController<TodoItem>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            ramp_festivalContext context = new ramp_festivalContext();
            DomainManager = new EntityDomainManager<TodoItem>(context, Request, Services, enableSoftDelete: true);
        }

        // GET tables/TodoItem
        public IQueryable<TodoItem> GetAllTodoItems()
        {
            return Query();
        }

        // GET tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<TodoItem> GetTodoItem(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<TodoItem> PatchTodoItem(string id, Delta<TodoItem> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/TodoItem
        public async Task<IHttpActionResult> PostTodoItem(TodoItem item)
        {
			TodoItem current = await InsertAsync(item);

			// Create a WNS native toast.
			WindowsPushMessage message = new WindowsPushMessage();

			// Define the XML paylod for a WNS native toast notification 
			// that contains the text of the inserted item.
			message.XmlPayload = @"<?xml version=""1.0"" encoding=""utf-8""?>" +
								 @"<toast><visual><binding template=""ToastText01"">" +
								 @"<text id=""1"">" + item.Text + @"</text>" +
								 @"</binding></visual></toast>";
			try
			{
				var result = await Services.Push.SendAsync(message);
				Services.Log.Info(result.State.ToString());
			}
			catch (System.Exception ex)
			{
				Services.Log.Error(ex.Message, null, "Push.SendAsync Error");
			}
			return CreatedAtRoute("Tables", new { id = current.Id }, current);
		}

        // DELETE tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteTodoItem(string id)
        {
            return DeleteAsync(id);
        }
    }
}