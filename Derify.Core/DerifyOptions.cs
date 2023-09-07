using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derify.Core
{
    public class DerifyOptions
    {
		private PathString pathMatch = "/Derify";

		public PathString PathMatch
		{
			get { return pathMatch; }
			set { pathMatch = value; }
		}

	}
}
