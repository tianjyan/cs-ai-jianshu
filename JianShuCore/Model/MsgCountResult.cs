using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JianShuCore.Model
{
    public class MsgCountResult
    {
        [JsonProperty("user-comment_on_note-comment")]
        public int UserCommentOnNoteComment
        {
            get;
            set;
        }

        [JsonProperty("comment-mention_somebody-user")]
        public int CommentMentionSomebodyUser
        {
            get;
            set;
        }

        [JsonProperty("user-like_something-note")]
        public int UserLikeSomethingNote
        {
            get;
            set;
        }

        [JsonProperty("user-like_something-user")]
        public int UserLikeSomethingUser
        {
            get;
            set;
        }

        [JsonProperty("user-like_something-collection")]
        public int UserLikeSomethingCollection
        {
            get;
            set;
        }

        [JsonProperty("user-like_something-notebook")]
        public int UserLikeSomethingNotebook
        {
            get;
            set;
        }

        [JsonProperty("note-recommend_by_editor")]
        public int NoteRecommendByEditor
        {
            get;
            set;
        }

        [JsonProperty("collection-approve_note-collectionsubmission")]
        public int CollectionApproveNoteCollectionsubmission
        {
            get;
            set;
        }

        [JsonProperty("collection-decline_note-collectionsubmission")]
        public int CollectionDeclineNoteCollectionsubmission
        {
            get;
            set;
        }

        [JsonProperty("collection-add_editor-user")]
        public int CollectionAddEditorUser
        {
            get;
            set;
        }

        [JsonProperty("collection-remove_editor-user")]
        public int CollectionRemoveEditorUser
        {
            get;
            set;
        }

        [JsonProperty("note-locked_by_editor")]
        public int NoteLockedByEditor
        {
            get;
            set;
        }

        [JsonProperty("collection-add_note-collectionnote")]
        public int CollectionAddNoteCollectionnote
        {
            get;
            set;
        }

        [JsonProperty("collection-contribute_note-collectionsubmission")]
        public int CollectionContributeNoteCollectionsubmission
        {
            get;
            set;
        }
    }
}
