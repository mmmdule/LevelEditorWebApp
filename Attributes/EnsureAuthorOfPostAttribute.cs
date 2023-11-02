using LevelEditorWebApp.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace LevelEditorWebApp.Attributes {
    public class EnsureAuthorOfPostAttribute : AuthorizeAttribute {
       
    }
}
