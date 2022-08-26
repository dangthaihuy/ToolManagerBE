using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Linq.Expressions;
using Manager.WebApp.Resources;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Html;

namespace Manager.WebApp.Helpers
{
    public static class HtmlHelpers
    {

        #region Extensions

        /// <summary>
        /// https://gist.github.com/scottcate/4469809
        /// 
        /// Usage:
        /// @Html.DescriptionFor(m => m.PropertyName)
        /// 
        /// supply cssclass name, and override span with div tag
        /// @Html.DescriptionFor(m => m.PropertyName, "desc", "div")
        /// 
        /// using the named param
        /// @Html.DescriptionFor(m => m.PropertyName, tagName: "div")
        /// </summary>
        public static MvcHtmlString DescriptionFor<TModel, TValue>(this HtmlHelper<TModel> self,
                                                                Expression<Func<TModel, TValue>> expression,
                                                                string cssClassName = "", string tagName = "span")
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, self.ViewData);
            var description = metadata.Description;

            if (!string.IsNullOrEmpty(description))
            {
                var tag = new TagBuilder(tagName) { InnerHtml = description };
                if (!string.IsNullOrEmpty(cssClassName))
                    tag.AddCssClass(cssClassName);

                return new MvcHtmlString(tag.ToString());
            }

            return MvcHtmlString.Empty;
        }



        public static MvcHtmlString HelpButtonFor<TModel, TValue>(this HtmlHelper<TModel> self,
                                                                Expression<Func<TModel, TValue>> expression,
                                                                string cssClassName = "", string tagName = "span", string place = "", string dataSkin = "")
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, self.ViewData);
            var description = metadata.Description;

            if (!string.IsNullOrEmpty(description))
            {
                //var tag = new TagBuilder(tagName) { InnerHtml = "?" };
                var tag = new TagBuilder(tagName) { InnerHtml = "" };
                cssClassName = "help-button " + cssClassName;
                tag.AddCssClass(cssClassName);

                //tag.MergeAttribute("data-rel", "tooltip");
                tag.MergeAttribute("data-toggle", "m-tooltip");
                tag.MergeAttribute("data-width", "auto");
                tag.MergeAttribute("data-original-title", description);
                tag.MergeAttribute("data-skin", dataSkin);
                tag.MergeAttribute("data-placement", place);

                //tag.MergeAttribute("data-placement", "right");
                //tag.MergeAttribute("data-original-title", description);

                return new MvcHtmlString(tag.ToString());
            }

            return MvcHtmlString.Empty;
        }

        #endregion


        public static IEnumerable<SelectListItem> TimeZoneList(string currentTimeZoneId)
        {
            var timeZoneList = TimeZoneInfo
            .GetSystemTimeZones()
            .Select(t => new SelectListItem
            {
                Text = t.DisplayName,
                Value = t.Id,
                Selected = !string.IsNullOrEmpty(currentTimeZoneId) && t.Id == currentTimeZoneId
            });

            return timeZoneList;
        }
        
        #region BootstrapPager

        //http://weblogs.asp.net/imranbaloch/a-simple-bootstrap-pager-html-helper

        public static HtmlString BootstrapPager(int currentPageIndex, Func<int, string> action, int totalItems, int pageSize = 10, int numberOfLinks = 5, string onclickEvent = "")
        {
            if (totalItems <= 0)
            {
                return HtmlString.Empty;
            }
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var lastPageNumber = (int)Math.Ceiling((double)currentPageIndex / numberOfLinks) * numberOfLinks;
            var firstPageNumber = lastPageNumber - (numberOfLinks - 1);
            var hasPreviousPage = currentPageIndex > 1;
            var hasNextPage = currentPageIndex < totalPages;
            if (lastPageNumber > totalPages)
            {
                lastPageNumber = totalPages;
            }
            var ul = new TagBuilder("ul");
            //ul.AddCssClass("pagination");
            ul.AddCssClass("m-datatable__pager-nav");
            ul.InnerHtml += AddLink(1, action, currentPageIndex == 1, "m-datatable__pager-link m-datatable__pager-link--first m-datatable__pager-link--disabled", "<<", ManagerResource.LB_FIRST_PAGE, "<i class='la la-angle-double-left'></i>", onclickEvent);
            ul.InnerHtml += AddLink(currentPageIndex - 1, action, !hasPreviousPage, "m-datatable__pager-link m-datatable__pager-link--first m-datatable__pager-link--disabled", "<", ManagerResource.LB_PREVIOUS_PAGE, "<i class='la la-angle-left'></i>", onclickEvent);
            for (int i = firstPageNumber; i <= lastPageNumber; i++)
            {
                ul.InnerHtml += AddLink(i, action, i == currentPageIndex, "m-datatable__pager-link m-datatable__pager-link-number m-datatable__pager-link--active", i.ToString(), i.ToString(), onclickEvent);
            }
            ul.InnerHtml += AddLink(currentPageIndex + 1, action, !hasNextPage, "m-datatable__pager-link m-datatable__pager-link--first m-datatable__pager-link--disabled", ">", ManagerResource.LB_NEXT_PAGE, "<i class='la la-angle-right'></i>", onclickEvent);
            ul.InnerHtml += AddLink(totalPages, action, currentPageIndex == totalPages, "m-datatable__pager-link m-datatable__pager-link--first m-datatable__pager-link--disabled", ">>", ManagerResource.LB_LAST_PAGE, "<i class='la la-angle-double-right'></i>", onclickEvent);
            
            return new HtmlString(ul.ToString());
        }

        private static TagBuilder AddLink(int index, Func<int, string> action, bool condition, string classToAdd, string linkText, string tooltip, string onclickEvent = "")
        {
            var li = new TagBuilder("li");
            li.MergeAttribute("title", tooltip);

            var a = new TagBuilder("a");
            if (condition)
            {
                a.AddCssClass(classToAdd);
            }
            else
            {
                if (!string.IsNullOrEmpty(onclickEvent))
                    li.MergeAttribute("onclick", onclickEvent + "(" + index + ")");

                a.AddCssClass("m-datatable__pager-link m-datatable__pager-link-number");
            }
            a.MergeAttribute("href", !condition ? action(index) : "javascript:");
            a.SetInnerText(linkText);
            li.InnerHtml = a.ToString();
            return li;
        }

        private static TagBuilder AddLink(int index, Func<int, string> action, bool condition, string classToAdd, string linkText, string tooltip, string linkIconHtml = "", string onclickEvent = "")
        {
            //var li = new TagBuilder("li");
            //li.MergeAttribute("title", tooltip);

            //var a = new TagBuilder("a");
            //a.AddCssClass("pager-control");

            //a.MergeAttribute("href", !condition ? action(index) : "javascript:");
            //a.MergeAttribute("data-page", index.ToString());
            //if (!string.IsNullOrEmpty(linkIconHtml))
            //    a.InnerHtml = linkIconHtml;
            //else
            //    a.SetInnerText(linkText);

            //if (condition)
            //{
            //    li.AddCssClass(classToAdd);
            //}

            //li.InnerHtml = a.ToString();
            //return li;

            var li = new TagBuilder("li");
            li.MergeAttribute("title", tooltip);

            var a = new TagBuilder("a");
            if (condition)
            {
                a.AddCssClass(classToAdd);
            }
            else
            {
                if (!string.IsNullOrEmpty(onclickEvent))
                    li.MergeAttribute("onclick", onclickEvent + "(" + index + ")");
                a.AddCssClass("m-datatable__pager-link m-datatable__pager-link-number");
            }
            a.MergeAttribute("href", !condition ? action(index) : "javascript:");

            if (!string.IsNullOrEmpty(linkIconHtml))
                a.InnerHtml = linkIconHtml;
            else
                a.SetInnerText(linkText);

            li.InnerHtml = a.ToString();
            return li;
        }
        #endregion

        public static string RemoveScriptTags(string htmlText)
        {
            if (string.IsNullOrEmpty(htmlText))
                return htmlText;

            var cleanStr = htmlText;

            cleanStr = Regex.Replace(cleanStr, "<script[\\d\\D]*?>[\\d\\D]*?</script>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            return cleanStr;
        }

        public static string ShowHtmlTextByLimit(string content, int maxLength = 120)
        {
            if (!string.IsNullOrEmpty(content))
            {
                content = HtmlRemoval.StripTagsCharArray(content);
                if (content.Length > maxLength)
                {
                    content = content.Substring(0, maxLength) + "...";
                }
            }

            return content;
        }
    }

    public static class HtmlRemoval
    {
        /// <summary>
        /// Remove HTML from string with Regex.
        /// </summary>
        public static string StripTagsRegex(string source)
        {
            return Regex.Replace(source, "<.*?>", string.Empty);
        }

        /// <summary>
        /// Compiled regular expression for performance.
        /// </summary>
        static Regex _htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);

        /// <summary>
        /// Remove HTML from string with compiled Regex.
        /// </summary>
        public static string StripTagsRegexCompiled(string source)
        {
            return _htmlRegex.Replace(source, string.Empty);
        }

        /// <summary>
        /// Remove HTML tags from string using char array.
        /// </summary>
        public static string StripTagsCharArray(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }

        public static bool IsHtmlFragment(string value)
        {
            return Regex.IsMatch(value, @"</?(p|div)>");
        }

        /// <summary>
        /// Remove tags from a html string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveTags(string value)
        {
            if (value != null)
            {
                value = CleanHtmlComments(value);
                value = CleanHtmlBehaviour(value);
                value = Regex.Replace(value, @"</[^>]+?>", " ");
                value = Regex.Replace(value, @"<[^>]+?>", "");
                value = value.Trim();
            }
            return value;
        }

        /// <summary>
        /// Clean script and styles html tags and content
        /// </summary>
        /// <returns></returns>
        public static string CleanHtmlBehaviour(string value)
        {
            value = Regex.Replace(value, "(<style.+?</style>)|(<script.+?</script>)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            return value;
        }

        /// <summary>
        /// Replace the html commens (also html ifs of msword).
        /// </summary>
        public static string CleanHtmlComments(string value)
        {
            //Remove disallowed html tags.
            value = Regex.Replace(value, "<!--.+?-->", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            return value;
        }

        /// <summary>
        /// Adds rel=nofollow to html anchors
        /// </summary>
        public static string HtmlLinkAddNoFollow(string value)
        {
            return Regex.Replace(value, "<a[^>]+href=\"?'?(?!#[\\w-]+)([^'\">]+)\"?'?[^>]*>(.*?)</a>", "<a href=\"$1\" rel=\"nofollow\" target=\"_blank\">$2</a>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        public static string StripHTML(string source)
        {
            try
            {
                string result;

                // Remove HTML Development formatting
                // Replace line breaks with space
                // because browsers inserts space
                result = source.Replace("\r", " ");
                // Replace line breaks with space
                // because browsers inserts space
                result = result.Replace("\n", " ");
                // Remove step-formatting
                result = result.Replace("\t", string.Empty);
                // Remove repeating spaces because browsers ignore them
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"( )+", " ");

                // Remove the header (prepare first by clearing attributes)
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*head([^>])*>", "<head>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"(<( )*(/)( )*head( )*>)", "</head>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(<head>).*(</head>)", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // remove all scripts (prepare first by clearing attributes)
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*script([^>])*>", "<script>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"(<( )*(/)( )*script( )*>)", "</script>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                //result = System.Text.RegularExpressions.Regex.Replace(result,
                //         @"(<script>)([^(<script>\.</script>)])*(</script>)",
                //         string.Empty,
                //         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"(<script>).*(</script>)", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // remove all styles (prepare first by clearing attributes)
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*style([^>])*>", "<style>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"(<( )*(/)( )*style( )*>)", "</style>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(<style>).*(</style>)", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // insert tabs in spaces of <td> tags
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*td([^>])*>", "\t",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // insert line breaks in places of <BR> and <LI> tags
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*br( )*>", "\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*li( )*>", "\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // insert line paragraphs (double line breaks) in place
                // if <P>, <DIV> and <TR> tags
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*div([^>])*>", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*tr([^>])*>", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*p([^>])*>", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // Remove remaining tags like <a>, links, images,
                // comments etc - anything that's enclosed inside < >
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<[^>]*>", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // replace special characters:
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @" ", " ",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&bull;", " * ",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&lsaquo;", "<",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&rsaquo;", ">",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&trade;", "(tm)",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&frasl;", "/",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&lt;", "<",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&gt;", ">",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&copy;", "(c)",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&reg;", "(r)",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Remove all others. More can be added, see
                // http://hotwired.lycos.com/webmonkey/reference/special_characters/
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&(.{2,6});", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // for testing
                //System.Text.RegularExpressions.Regex.Replace(result,
                //       this.txtRegex.Text,string.Empty,
                //       System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // make line breaking consistent
                result = result.Replace("\n", "\r");

                // Remove extra line breaks and tabs:
                // replace over 2 breaks with 2 and over 4 tabs with 4.
                // Prepare first to remove any whitespaces in between
                // the escaped characters and remove redundant tabs in between line breaks
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\r)( )+(\r)", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\t)( )+(\t)", "\t\t",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\t)( )+(\r)", "\t\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\r)( )+(\t)", "\r\t",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Remove redundant tabs
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\r)(\t)+(\r)", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Remove multiple tabs following a line break with just one tab
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\r)(\t)+", "\r\t",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Initial replacement target string for line breaks
                string breaks = "\r\r\r";
                // Initial replacement target string for tabs
                string tabs = "\t\t\t\t\t";
                for (int index = 0; index < result.Length; index++)
                {
                    result = result.Replace(breaks, "\r\r");
                    result = result.Replace(tabs, "\t\t\t\t");
                    breaks = breaks + "\r";
                    tabs = tabs + "\t";
                }

                // That's it.
                return result;
            }
            catch
            {
                return source;
            }
        }
    }
}