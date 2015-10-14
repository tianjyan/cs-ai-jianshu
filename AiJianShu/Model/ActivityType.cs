using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiJianShu.Model
{
    public enum ActivityType
    {
        None,

        FollowUser,
        LikeNote,
        LikeNoteBook,
        LikeCollection,

        Comment,//comment_on_note
        Created,
        Note,//note
    }
}
