export interface Video
{
    // posterUrl: string;
    videoId:number;
    videoName:string;
    typeId:number;
    seriesId:number;
    mainGenreId:number;
    seasonId:number;
    episode?:number|undefined;
    runningTime:string;
    isShowing:boolean;
    releaseDate:Date;
    rating:number;
    popularity:number;
    thumbnailPath:string;
    lang:string;
    summary:string;
    views:number;
    ageRating:string;
    trailerUrl:string;
    mainGenreName:string;
    seasonName:string;
    bgpath:string;
}
