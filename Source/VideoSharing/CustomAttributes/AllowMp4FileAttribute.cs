namespace VideoSharing.CustomAttributes
{
    public class AllowMp4FileAttribute : BaseAllowFileTypeAttribute
    {
        public AllowMp4FileAttribute() : base(MimeDetective.Definitions.Default.FileTypes.Video.MP4())
        {
        }
    }
}