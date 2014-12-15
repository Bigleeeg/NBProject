using JIAOFENG.Practices.Library.Common;
using JIAOFENG.Practices.Library.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace JIAOFENG.Practices.Library.Mvc
{
    public static class MvcExtension
    {
        public static List<SelectListItem> ToSelectList<T>(this List<T> list) where T : ISelectItem
        {
            List<SelectListItem> selectList = new List<SelectListItem>();

            SelectListItem itemSelectAll = new SelectListItem();
            itemSelectAll.Value = Constant.SelectAll;
            itemSelectAll.Text = Constant.SelectAllText;
            selectList.Add(itemSelectAll);
            foreach (T t in list)
            {
                SelectListItem item = new SelectListItem();
                item.Value = t.Value;
                item.Text = t.Text;
                selectList.Add(item);
            }
            return selectList;
        }
        public static JsonResult ToJsonResult(this ReturnResult returnResult, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.AllowGet)
        {
            JsonResult jsonResult = new JsonResult();
            jsonResult.Data = new { result = returnResult.Result, code = (int)returnResult.Code, message = returnResult.Message, other =returnResult.Other };
            jsonResult.JsonRequestBehavior = jsonRequestBehavior;
            return jsonResult;
        }
    }
}
