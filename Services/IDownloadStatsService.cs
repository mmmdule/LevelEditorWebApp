using LevelEditorWebApp.Models;

namespace LevelEditorWebApp.Services {
    public interface IDownloadStatsService {
        public Task AddDownload(int postId);
        public int GetPostDownloads(int postId);
        public Post[] GetMostDownloadedPostsAllTime(int count);
    }
}
