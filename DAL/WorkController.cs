using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Service.Controllers
{
    [Route("api/[controller]")]
    public class WorkController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public WorkController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpGet]
        public ActionResult Get()
        {
            return Json(DAL.WorkInfo.Instance.GetCount());
        }
        [HttpGet("new")]
        public ActionResult GetNew()
        {
            var result = DAL.WorkInfo.Instance.GetNew();
            if (result.Count() != 0)
                return Json(Result.Ok(result));
            else
                return Json(Result.Err("记录数为0"));
        }
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var result = DAL.WorkInfo.Instance.GetModel(id);
            if (result != null)
                return Json(Result.Ok(result));
            else
                return Json(Result.Err("WorkId不存在"));
        }
        [HttpPost]
        public ActionResult Post([FromBody]Model.WorkInfo workInfo)
        {
            workInfo.recommend = "否";
            workInfo.workVerify = "待审核";
            workInfo.uploadTime = DateTime.Now;
            try
            {
                int n = DAL.WorkInfo.Instance.Add(workInfo);
                return Json(Result.Ok("发布作品成功", n));

            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("foreign key"))
                    if (ex.Message.ToLower().Contains("username"))
                        return Json(Result.Err("合法用户才能添加记录"));
                    else
                        return Json(Result.Err("作者所属活动不存在"));
                else if (ex.Message.ToLower().Contains("null"))
                    return Json(Result.Err("作者名称，作者图片，上传作品时间，作品审核情况，用户名，是否推荐不能为空"));
                else
                    return Json(Result.Err(ex.Message));

            }
        }
        [HttpPut]
        public ActionResult Put([FromBody] Model.WorkInfo workInfo)
        {
            workInfo.recommend = "否";
            workInfo.workVerify = "待审核";
            workInfo.uploadTime = DateTime.Now;
            try
            {
                int n = DAL.WorkInfo.Instance.Update(workInfo);
                if (n != 0)
                    return Json(Result.Ok("修改作品成功", workInfo.workId));
                else
                    return Json(Result.Err("workId不存在"));


            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("null"))
                    return Json(Result.Err("作者名称，作者图片，上传作品时间，作品审核情况，用户名，是否推荐不能为空"));
                else
                    return Json(Result.Err(ex.Message));

            }
        }
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var n = DAL.WorkInfo.Instance.Delete(id);
                if (n != 0)
                    return Json(Result.Ok("删除成功"));
                else
                    return Json(Result.Err("workId不存在"));
            }
            catch (Exception ex)
            {
                return Json(Result.Err(ex.Message));
            }
        }
        [HttpPost("count")]
        public ActionResult GetCount([FromBody]int[] activityIds)
        {
            return Json(DAL.WorkInfo.Instance.GetCount(activityIds));
        }
        [HttpPost("page")]
        public ActionResult getPage([FromBody]Model.WorkPage page)
        {
            var result = DAL.WorkInfo.Instance.GetPage(page);
            if (result.Count() == 0)
                return Json(Result.Err("返回记录数为0"));
            else
                return Json(Result.Ok(result));
        }
        [HttpGet("findCount")]
        public ActionResult getFindCount(string findName)
        {
            if (findName == null) findName = "";
            return Json(DAL.WorkInfo.Instance.GetFindCount(findName));
        }
        [HttpGet("myCount")]
        public ActionResult getMyCount(string userName)
        {
            if (userName == null) userName = "";
            return Json(DAL.WorkInfo.Instance.GetMyCount(userName));
        }
        [HttpPost("findPage")]
        public ActionResult getFindPage([FromBody]Model.WorkFindPage page)
        {
            if (page.workName == null) page.workName = "";
            var result = DAL.WorkInfo.Instance.GetFindPage(page);
            if (result.Count() == 0)
                return Json(Result.Err("返回记录数为0"));
            else
                return Json(Result.Ok(result));
        }
        [HttpPost("myPage")]
        public ActionResult getMyPage([FromBody]Model.WorkMyPage page)
        {
            if (page.userName == null) page.userName = "";
            var result = DAL.WorkInfo.Instance.GetMyPage(page);
            if (result.Count() == 0)
                return Json(Result.Err("返回记录数为0"));
            else
                return Json(Result.Ok(result));

        }
    }
}
