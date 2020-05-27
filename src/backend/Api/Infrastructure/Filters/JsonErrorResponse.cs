using System.Collections.Generic;

namespace Egl.Sit.Api.Infrastructure.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonErrorResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public JsonErrorResponse()
        {
            _messages = new Dictionary<string, ICollection<string>>();
        }

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, ICollection<string>> _messages;
        public IReadOnlyDictionary<string, ICollection<string>> Messages => _messages;
        public void AddMessage(string key, string message)
        {
            var succeed = _messages.TryGetValue(key, out ICollection<string> messages);

            if(!succeed)
            {
                _messages.Add(key, new List<string> { message });
            }
            else
            {
                messages.Add(message);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public object DeveloperMeesage { get; set; }
    }
}