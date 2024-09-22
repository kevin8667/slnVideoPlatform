export interface CreateVideoDTO
{
    videoName: string;
    typeId: number;
    seriesId: number;
    mainGenreId: number;
    seasonId?: number;
    episode?: number;
    runningTime?: string;
    isShowing: boolean;
    releaseDate: string;
    rating: number;
    popularity: number;
    thumbnailPath?: string;
    lang: string;
    summary?: string;
    ageRating?: string;
    trailerUrl?: string;
    bgpath?:string;
    images?: ImageDTO[]; // 新增圖片資訊
}

export interface ImageDTO {
    imagePath: string;
}