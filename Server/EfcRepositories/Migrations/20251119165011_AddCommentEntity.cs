using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EfcRepositories.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comments_posts_CommentsId",
                table: "comments");

            migrationBuilder.DropForeignKey(
                name: "FK_comments_users_UserCommentsId",
                table: "comments");

            migrationBuilder.DropForeignKey(
                name: "FK_posts_users_PostsId",
                table: "posts");

            migrationBuilder.RenameColumn(
                name: "PostsId",
                table: "posts",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_posts_PostsId",
                table: "posts",
                newName: "IX_posts_UserId");

            migrationBuilder.RenameColumn(
                name: "UserCommentsId",
                table: "comments",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "CommentsId",
                table: "comments",
                newName: "PostId");

            migrationBuilder.RenameIndex(
                name: "IX_comments_UserCommentsId",
                table: "comments",
                newName: "IX_comments_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_comments_CommentsId",
                table: "comments",
                newName: "IX_comments_PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_comments_posts_PostId",
                table: "comments",
                column: "PostId",
                principalTable: "posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_comments_users_UserId",
                table: "comments",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_posts_users_UserId",
                table: "posts",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comments_posts_PostId",
                table: "comments");

            migrationBuilder.DropForeignKey(
                name: "FK_comments_users_UserId",
                table: "comments");

            migrationBuilder.DropForeignKey(
                name: "FK_posts_users_UserId",
                table: "posts");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "posts",
                newName: "PostsId");

            migrationBuilder.RenameIndex(
                name: "IX_posts_UserId",
                table: "posts",
                newName: "IX_posts_PostsId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "comments",
                newName: "UserCommentsId");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "comments",
                newName: "CommentsId");

            migrationBuilder.RenameIndex(
                name: "IX_comments_UserId",
                table: "comments",
                newName: "IX_comments_UserCommentsId");

            migrationBuilder.RenameIndex(
                name: "IX_comments_PostId",
                table: "comments",
                newName: "IX_comments_CommentsId");

            migrationBuilder.AddForeignKey(
                name: "FK_comments_posts_CommentsId",
                table: "comments",
                column: "CommentsId",
                principalTable: "posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_comments_users_UserCommentsId",
                table: "comments",
                column: "UserCommentsId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_posts_users_PostsId",
                table: "posts",
                column: "PostsId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
