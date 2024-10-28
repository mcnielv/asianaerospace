using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AAC.UI.Website
{
    public class AuthorizeAdminOnly : AuthorizeAttribute
    {
        protected override bool AuthorizeCore( HttpContextBase httpContext )
        {
            var authorized = base.AuthorizeCore( httpContext );
            if ( !authorized )
            {
                // The user is not authenticated
                return false;
            }

            //var user = httpContext.User;
            //user.
            //if ( user.use .IsInRole( "Admin" ) )
            //{
            //    // Administrator => let him in
            //    return true;
            //}

            var rd = httpContext.Request.RequestContext.RouteData;
            var id = rd.Values["id"] as string;
            if ( string.IsNullOrEmpty( id ) )
            {
                // No id was specified => we do not allow access
                return false;
            }
            else
            {

            }
            return false;

            //return IsOwnerOfPost( user.Identity.Name, id );
        }

        private bool IsOwnerOfPost( string username, string postId )
        {
            // TODO: you know what to do here
            throw new NotImplementedException();
        }
    } 
}