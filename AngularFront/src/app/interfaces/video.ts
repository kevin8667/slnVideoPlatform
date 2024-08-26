
export interface Video
{
    videoName:string;
    typeID:number;
    seriesID:number;
    mainGenreID:number;
    seasonID:number;
    episode:number;
    runningTime:string;
    isShowing:boolean;
    releaseDate:Date;
    rating:number;
    popularity:number;
    thumbnailID:number;
    lang:string;
    summary:string;
    views:number;
    ageRating:string;
    trailerUrl:string;
}