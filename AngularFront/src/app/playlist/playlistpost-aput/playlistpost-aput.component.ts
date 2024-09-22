import { Component, ChangeDetectorRef, OnInit, EventEmitter, Output } from '@angular/core';
import { PlaylistDTO } from '../../interfaces/PlaylistDTO';
import { PlaylistitemDTO } from '../../interfaces/PlaylistitemDTO';
import { MemberInfoDTO } from '../../interfaces/MemberInfoDTO';
import { PlaylistService } from '../../services/playlist.service';
import { CdkDragDrop } from '@angular/cdk/drag-drop';
import { VideoListDTO } from 'src/app/interfaces/VideoListDTO';
import { PlayListCreateDTO } from '../../interfaces/PlayListCreateDTO';
import { Router } from '@angular/router';

@Component({
  selector: 'app-playlistpost-aput',
  templateUrl: './playlistpost-aput.component.html',
  styleUrls: ['./playlistpost-aput.component.css'],
})
export class PlaylistpostAputComponent implements OnInit {
  @Output() playlistAdded: EventEmitter<void> = new EventEmitter<void>();
  isEditing: boolean = false;
  displayDialog: boolean = false;

  playlist: PlaylistDTO = {
    playListId: 0,
    playListName: '',
    playListDescription: '',
    viewCount: 100,
    likeCount: 100,
    addedCount: 100,
    sharedCount: 100,
    showImage: null,
  };

  playlistItems: PlaylistitemDTO[] = [];
  playlistCollaborators: MemberInfoDTO[] = [];
  availableCollaborators: MemberInfoDTO[] = [];
  allAvailableVideos: VideoListDTO[] = [];
  selectedVideos: VideoListDTO[] = [];

  imagePreview: string | ArrayBuffer | null = '/assets/img/noimageooo.jpg';

  constructor(
    private cdr: ChangeDetectorRef,
    private playlistService: PlaylistService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadAllAvailableVideos();
  }

  getImagePreview(): string {
    return this.imagePreview
      ? (this.imagePreview as string)
      : '/assets/img/noimageooo.jpg';
  }

  showDialog(isEditing: boolean, playlist?: PlaylistDTO): void {
    this.isEditing = isEditing;
    this.displayDialog = true;

    if (isEditing && playlist) {
      this.playlist = { ...playlist };
      this.imagePreview = this.playlist.showImage
        ? 'data:image/png;base64,' + this.playlist.showImage
        : '/assets/img/noimageooo.jpg';
      this.loadCollaborators();
      this.loadPlaylistItems();
    } else {
      this.resetForm();
      this.loadCollaborators();
      this.loadAllAvailableVideos();
    }
    this.cdr.detectChanges();
  }

  loadAllAvailableVideos(): void {
    this.playlistService.getAllVideos().subscribe(
      (videos: VideoListDTO[]) => {
        this.allAvailableVideos = videos;
      },
      (error) => {
        console.error('Error loading available videos', error);
      }
    );
  }

  loadPlaylistItems(): void {
    this.playlistService
      .getPlaylistItems(this.playlist.playListId ?? 0)
      .subscribe(
        (items: PlaylistitemDTO[]) => {
          this.playlistItems = items.sort(
            (a, b) => a.videoPosition - b.videoPosition
          );

          this.selectedVideos = this.playlistItems.map((item) => ({
            videoId: item.videoId,
            videoName: item.videoName,
            episode: item.episode,
            thumbnailPath: item.thumbnailPath ?? '',
          }));
        },
        (error) => {
          console.error('Error loading playlist items', error);
        }
      );
  }

  loadCollaborators(): void {
    this.playlistService.getCollaborators().subscribe(
      (collaborators) => {
        this.availableCollaborators = collaborators;
        if (this.isEditing && this.playlist.playListId) {
          this.playlistService
            .getCollaborators(this.playlist.playListId)
            .subscribe(
              (selectedCollaborators) => {
                this.playlistCollaborators = this.availableCollaborators.filter(
                  (collaborator) =>
                    selectedCollaborators.some(
                      (sc) => sc.memberId === collaborator.memberId
                    )
                );
              },
              (error) => {
                console.error('Error loading selected collaborators', error);
              }
            );
        }
      },
      (error) => {
        console.error('Error loading all collaborators', error);
      }
    );
  }

  onVideoSelectionChange(event: any): void {
    this.selectedVideos = event.value;
    this.selectedVideos.forEach((video) => this.onSelectVideo(video));
  }

  resetForm(): void {
    this.playlist = {
      playListId: 0,
      playListName: '',
      playListDescription: '',
      viewCount: 100,
      likeCount: 100,
      addedCount: 100,
      sharedCount: 100,
      showImage: null,
    };
    this.imagePreview = '/assets/img/noimageooo.jpg';
    this.playlistItems = [];
    this.playlistCollaborators = [];
    this.selectedVideos = [];
  }

  onCancel(): void {
    this.displayDialog = false;
  }

  defaultPlaylistNamePlaceholder(): string {
    const today = new Date();
    const yyyy = today.getFullYear();
    const mm = String(today.getMonth() + 1).padStart(2, '0');
    const dd = String(today.getDate()).padStart(2, '0');
    return `片單${yyyy}-${mm}-${dd}`;
  }

  defaultPlaylistDescriptionPlaceholder(): string {
    const today = new Date();
    const yyyy = today.getFullYear();
    const mm = String(today.getMonth() + 1).padStart(2, '0');
    const dd = String(today.getDate()).padStart(2, '0');
    return `該片單建立於${yyyy}-${mm}-${dd}`;
  }

  onSubmit(): void {
    if (!this.isEditing) {
      if (this.playlist.playListName.trim() === '') {
        this.playlist.playListName = this.defaultPlaylistNamePlaceholder();
      }
      if (this.playlist.playListDescription.trim() === '') {
        this.playlist.playListDescription =
          this.defaultPlaylistDescriptionPlaceholder();
      }
    }

    if (
      this.playlist.playListName.trim() === '' ||
      this.playlist.playListDescription.trim() === ''
    ) {
      return;
    }

    const collaboratorIds = this.playlistCollaborators.map((c) => c.memberId);
    const videoIds = this.selectedVideos.map((video) => video.videoId);

    const playListCreateDTO: PlayListCreateDTO = {
      PlayList: this.playlist,
      videoIds: videoIds,
      collaboratorIds: collaboratorIds,
    };

    if (this.isEditing) {
      this.playlistService
        .editPlaylist(this.playlist.playListId ?? 0, playListCreateDTO)
        .subscribe(
          (response) => {
            console.log('播放清單已編輯');
            this.playlistAdded.emit();
          },
          (error) => {
            console.error('編輯播放清單時發生錯誤', error);
          }
        );
    } else {
      this.playlistService.addNewPlaylist(playListCreateDTO).subscribe(
        (response) => {
          console.log('播放清單已新增');
          this.playlistAdded.emit();
        },
        (error) => {
          console.error('新增播放清單時發生錯誤', error);
        }
      );
    }

    this.displayDialog = false;
  }

  onImageError(event: any) {
    event.target.src = '/assets/img/memberooo.png';
  }

  onImageSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = () => {
        this.imagePreview = reader.result;
        this.cdr.detectChanges();
      };
      reader.readAsDataURL(file);
      reader.onloadend = () => {
        const base64String = (reader.result as string).split(',')[1];
        this.playlist.showImage = base64String;
        this.cdr.detectChanges();
      };
    }
  }

  drop(event: CdkDragDrop<PlaylistitemDTO[]>): void {
    console.log('Previous Index:', event.previousIndex);
    console.log('Current Index:', event.currentIndex);

    const movedVideo = this.playlistItems.splice(event.previousIndex, 1)[0];
    this.playlistItems.splice(event.currentIndex, 0, movedVideo);

    this.playlistItems.forEach((video, index) => {
      video.videoPosition = index + 1;
      console.log(
        `Updating position for videoId: ${video.videoId}, newPosition: ${video.videoPosition}`
      );

      this.playlistService
        .updateVideoPosition(
          this.playlist.playListId ?? 0,
          video.videoId,
          video.videoPosition
        )
        .subscribe(
          () => {
            console.log('Video position updated');
          },
          (error) => {
            console.error('Error updating video position:', error);
          }
        );
    });

    this.playlistItems.sort((a, b) => a.videoPosition - b.videoPosition);

    this.selectedVideos = this.playlistItems.map((item) => ({
      videoId: item.videoId,
      videoName: item.videoName,
      episode: item.episode,
      thumbnailPath: item.thumbnailPath ?? '',
    }));

    this.cdr.detectChanges();
  }

  updateVideoPositions(): void {
    if (!this.playlist.playListId) {
      console.error('Invalid playlist ID');
      return;
    }

    this.playlistItems.forEach((item, index) => {
      console.log(
        `Updating videoId: ${item.videoId}, newPosition: ${index + 1}`
      );
      this.playlistService
        .updateVideoPosition(
          this.playlist.playListId ?? 0,
          item.videoId,
          index + 1
        )
        .subscribe(
          (response) => {
            console.log('影片位置已更新', response);
          },
          (error) => {
            console.error('Error updating video position', error);
          }
        );
    });
  }

  updatePlaylistItems(): void {
    if (this.playlist.playListId) {
      this.playlistService
        .addPlaylistItems(this.playlist.playListId, this.playlistItems)
        .subscribe(
          (response) => {
            console.log('播放清單項目已更新:', response);
          },
          (error) => {
            console.error('Error updating playlist items', error);
          }
        );
    }
  }

  onSelectVideo(item: VideoListDTO): void {
    const existingVideo = this.playlistItems.find(
      (video) => video.videoId === item.videoId
    );

    if (!existingVideo) {
      this.playlistItems.push({
        playListId: this.playlist.playListId ?? 0,
        videoId: item.videoId,
        videoPosition: this.playlistItems.length + 1,
        videoName: item.videoName,
        thumbnailPath: item.thumbnailPath,
        episode: item.episode ?? 0,
      });
    }
  }

  removePlaylistItem(index: number): void {
    const video = this.selectedVideos[index];
    this.playlistService
      .removePlaylistItem(this.playlist.playListId ?? 0, video.videoId)
      .subscribe(
        () => {
          this.selectedVideos.splice(index, 1);
          this.playlistItems.splice(index, 1);
        },
        (error) => {
          console.error('Error removing video from playlist:', error);
        }
      );
  }
}
