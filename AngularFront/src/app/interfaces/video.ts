
export interface Video
{
    videoId:number;
    videoName:string;
    typeId:number;
    seriesId:number;
    mainGenreId:number;
    seasonId:number;
    episode:number;
    runningTime:string;
    isShowing:boolean;
    releaseDate:Date;
    rating:number;
    popularity:number;
    thumbnailId:number;//
    lang:string;
    summary:string;
    views:number;
    ageRating:string;
    trailerUrl:string;
    mainGenreName:string;
    seasonName:string;
}
