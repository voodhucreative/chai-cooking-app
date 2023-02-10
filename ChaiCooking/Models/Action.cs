using System;
using System.Threading.Tasks;
using ChaiCooking.Helpers;

namespace ChaiCooking.Models
{
    public class Action
    {
        public string Name;
        public string Event;
        public int Id;
        public bool BlocksInput;
        public int Priority;
        public int TargetPageId { get; set; }

        public Action(int id, int targetPageId)
        {
            this.Id = id;
            this.Name = null;
            this.Event = null;
            this.BlocksInput = false;
            this.Priority = (int)Actions.PriorityName.Low;
            this.TargetPageId = targetPageId;
        }

        public Action(int id)
        {
            this.Id = id;
            this.Name = null;
            this.Event = null;
            this.BlocksInput = false;
            this.Priority = (int)Actions.PriorityName.Low;
            this.TargetPageId = -1;
        }

        public async Task<bool> Execute()
        {
            await App.PerformActionAsync(this);
            return true;
        }
    }
}
