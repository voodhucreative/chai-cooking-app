using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChaiCooking.Pages;

namespace ChaiCooking.Helpers
{
    public static class Pages
    {
        public static int CurrentPage;
        public static int NextPage;
        public static int LastPage;
        public static int PageBeforeMenu;
        public static int PagesOnStack;
        public static int TransitionAction;
        public static int TransitionDirection;


        public enum TransitionTypes
        {
            FadeIn,
            FadeOut,
            SlideInFromLeft,
            SlideInFromRight,
            SlideInFromTop,
            SlideInFromBottom,
            SlideOutToLeft,
            SlideOutToRight,
            SlideOutToTop,
            SlideOutToBottom,
            ScaleIn,
            ScaleOut,
            RotateIn,
            RotateOut,
            ScaleAndRotateIn,
            ScaleAndRotateOut,
            ScaleRotateAndFadeIn,
            ScaleRotateAndFadeOut
        };

        public enum TransitionActions
        {
            Next,
            Last,
            Direct
        };

        public enum TransitionDirections
        {
            Vertical,
            Horizontal
        };

        static List<Page> PageStack;

        public static void Init()
        {
            CurrentPage = 0;
            NextPage = 1;
            LastPage = 0;
            PageBeforeMenu = 0;
            PagesOnStack = 0;
            PageStack = new List<Page>();
            TransitionAction = (int)TransitionActions.Direct;
            TransitionDirection = (int)TransitionDirections.Horizontal;
        }

        public static void AddPage(Page page)
        {
            int index = page.Id;
            if (PageStack == null)
            {
                PageStack = new List<Page>();
            }

            PageStack.Add(page);
            PagesOnStack++;
            PrintPageStack();
        }

        public static void PrintPageStack()
        {
            Console.WriteLine("Pages on stack:");
            foreach (Page pageOnStack in PageStack)
            {
                Console.WriteLine("Page: " + pageOnStack.Id);
            }
        }

        public static void RemovePage(int index)
        {
            PageStack.RemoveAt(index);
        }

        public static Page GetPageById(int index)
        {
            int stackIndex = 0;
            foreach (Page page in PageStack)
            {
                if (index == page.Id)
                {
                    return PageStack[stackIndex];
                }
                stackIndex++;
            }
            return GetPageById(0);
        }

        public static void Reset()
        {
            CurrentPage = 0;
            NextPage = 1;
            LastPage = 0;
        }

        public static void SetCurrent(int pageId)
        {
            LastPage = CurrentPage;
            CurrentPage = pageId;
        }

        public static void SetNext(int pageId)
        {
            NextPage = pageId;
        }

        public static void SetLast(int pageId)
        {
            NextPage = LastPage;
            LastPage = pageId;
        }

        public static Page GetCurrent()
        {
            return GetPageById(CurrentPage);
        }

        public static int GetNextPageId()
        {
            return NextPage;
        }

        public static int GetLastPageId()
        {
            return LastPage;
        }

        public static int GetCurrentPageId()
        {
            return CurrentPage;
        }

        public static Page GetNext()
        {
            return GetPageById(NextPage);
        }

        public static Page GetLast()
        {
            return GetPageById(LastPage);
        }

        public static void SetCurrentPage(int pageId)
        {
            CurrentPage = pageId;
        }

        
        public static async void GoTo(int pageId)
        {
            NextPage = pageId;
            await Task.Delay(100);
        }
        /*
        public static async void Next()
        {
            if (CurrentPage < PageStack.Count - 2)
            {
                LastPage = CurrentPage;
                CurrentPage++;
                NextPage = CurrentPage + 1;
            }
            else
            {
                //LastPage = CurrentPage;
                //CurrentPage = 0;
                //NextPage =  0;
                //Debug.WriteLine("Last page reached");
            }

            await Task.Delay(100);
            //GetCurrent().PositionPage();
            //Debug.WriteLine("Go to next page");
        }

        public static async void Last()
        {
            if (CurrentPage > 0)
            {
                LastPage = CurrentPage;
                CurrentPage--;
                NextPage = CurrentPage + 1;
            }
            else
            {
                //Debug.WriteLine("Last page reached");
            }
            await Task.Delay(100);
        }*/

        public static async Task<bool> UpdateCurrent()
        {
            await Task.Delay(10);
            await GetCurrent().Update();
            return true;
        }

        public static async Task<bool> TransitionToNext()
        {
            await Task.Delay(100);
            return true;
        }

        public static async Task<bool> TransitionToLast()
        {
            await Task.Delay(100);
            return true;
        }
    }
}
