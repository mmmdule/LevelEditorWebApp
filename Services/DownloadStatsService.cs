using LevelEditorWebApp.Classes;
using LevelEditorWebApp.Data;
using LevelEditorWebApp.Models;

namespace LevelEditorWebApp.Services {
    public class DownloadStatsService : IDownloadStatsService {
        public ApplicationDbContext applicationDbContext;
        public DownloadStatsService(ApplicationDbContext applicationDbContext) {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task AddDownload(int postId) {
            PostDownloadInfo postDownloadInfo = applicationDbContext.PostDownloadInfo.Where(p => p.PostId == postId).FirstOrDefault();

            if(postDownloadInfo == null) {
                applicationDbContext.PostDownloadInfo.Add(new PostDownloadInfo() { PostId = postId, DownloadCount = 1 });
            } else {
                postDownloadInfo.DownloadCount++;
            }

            await applicationDbContext.SaveChangesAsync();
        }

        public Post[] GetMostDownloadedPostsAllTime(int count) {
            //get all posts and sort them by downloads, then return the first count posts
            //skip posts with no downloads
            applicationDbContext.Post.ToList().Sort((x, y) => GetPostDownloads(x.PostId).CompareTo(GetPostDownloads(y.PostId)));
            return applicationDbContext.Post.ToList().Where(p => GetPostDownloads(p.PostId) > 0).Take(count).ToArray();
        }

        public int GetPostDownloads(int postId) {
            PostDownloadInfo pdi = applicationDbContext.PostDownloadInfo.Where(p => p.PostId == postId).FirstOrDefault();

            return pdi is not null ? pdi.DownloadCount : 0;
        }
    }
}
