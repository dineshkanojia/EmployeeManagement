#pragma checksum "E:\NCI\Term2\SC\EmployeeManagement\EmployeeManagement\Views\ScheduleMeeting\MeetingData.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d05afb30b6a9b1283e5952b47b9ce2a47f28a176"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_ScheduleMeeting_MeetingData), @"mvc.1.0.view", @"/Views/ScheduleMeeting/MeetingData.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "E:\NCI\Term2\SC\EmployeeManagement\EmployeeManagement\Views\_ViewImports.cshtml"
using EmployeeManagement;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\NCI\Term2\SC\EmployeeManagement\EmployeeManagement\Views\_ViewImports.cshtml"
using EmployeeManagement.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d05afb30b6a9b1283e5952b47b9ce2a47f28a176", @"/Views/ScheduleMeeting/MeetingData.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3af35a328bfe0c906f75d04efeba14f80e1f583b", @"/Views/_ViewImports.cshtml")]
    public class Views_ScheduleMeeting_MeetingData : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<EmployeeManagement.Models.MeetingEvent>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "E:\NCI\Term2\SC\EmployeeManagement\EmployeeManagement\Views\ScheduleMeeting\MeetingData.cshtml"
  
    ViewData["Title"] = "MeetingData";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1>MeetingData</h1>\r\n\r\n<data>\r\n");
#nullable restore
#line 9 "E:\NCI\Term2\SC\EmployeeManagement\EmployeeManagement\Views\ScheduleMeeting\MeetingData.cshtml"
     foreach (var myevent in Model)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("    <event");
            BeginWriteAttribute("id", " id=\"", 195, "\"", 211, 1);
#nullable restore
#line 11 "E:\NCI\Term2\SC\EmployeeManagement\EmployeeManagement\Views\ScheduleMeeting\MeetingData.cshtml"
WriteAttributeValue("", 200, myevent.Id, 200, 11, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n        <start_date>\r\n            <![CDATA[");
#nullable restore
#line 13 "E:\NCI\Term2\SC\EmployeeManagement\EmployeeManagement\Views\ScheduleMeeting\MeetingData.cshtml"
                Write(String.Format("{0:MM/dd/yyyy HH:mm}", @myevent.StartDate));

#line default
#line hidden
#nullable disable
            WriteLiteral("]]>\r\n        </start_date>\r\n        <end_date>\r\n            <![CDATA[");
#nullable restore
#line 16 "E:\NCI\Term2\SC\EmployeeManagement\EmployeeManagement\Views\ScheduleMeeting\MeetingData.cshtml"
                Write(String.Format("{0:MM/dd/yyyy HH:mm}", @myevent.EndDate));

#line default
#line hidden
#nullable disable
            WriteLiteral("]]>\r\n        </end_date>\r\n        <text><![CDATA[");
#nullable restore
#line 18 "E:\NCI\Term2\SC\EmployeeManagement\EmployeeManagement\Views\ScheduleMeeting\MeetingData.cshtml"
                  Write(myevent.Description);

#line default
#line hidden
#nullable disable
            WriteLiteral("]]></text>\r\n        <useremail><![CDATA[");
#nullable restore
#line 19 "E:\NCI\Term2\SC\EmployeeManagement\EmployeeManagement\Views\ScheduleMeeting\MeetingData.cshtml"
                       Write(myevent.UserEmail);

#line default
#line hidden
#nullable disable
            WriteLiteral("]]></useremail>\r\n    </event>\r\n");
#nullable restore
#line 21 "E:\NCI\Term2\SC\EmployeeManagement\EmployeeManagement\Views\ScheduleMeeting\MeetingData.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("</data>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<EmployeeManagement.Models.MeetingEvent>> Html { get; private set; }
    }
}
#pragma warning restore 1591