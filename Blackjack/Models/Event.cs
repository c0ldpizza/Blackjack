using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blackjack.Models
{
    public class Event
    {
        private string name;
        private string webURI;
        private string imageURL;
        private string id;
        private string category;

        //properties
        #region
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public string WebURI
        {
            get
            {
                return webURI;
            }

            set
            {
                webURI = value;
            }
        }

        public string ImageURL
        {
            get
            {
                return imageURL;
            }

            set
            {
                imageURL = value;
            }
        }

        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public string Category
        {
            get
            {
                return category;
            }

            set
            {
                category = value;
            }
        }
        #endregion

        public Event() { }

        public Event(string name, string webURI, string imgURL, string id)
        {
            this.name = name;
            this.webURI = webURI;
            this.imageURL = imgURL;
            this.id = id;
        }
    }
}