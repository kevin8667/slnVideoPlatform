import { Component, ChangeDetectorRef, OnInit } from '@angular/core';
import { PlaylistDTO } from '../../interfaces/PlaylistDTO';
import { PlaylistitemDTO } from '../../interfaces/PlaylistitemDTO';
import { MemberInfoDTO } from '../../interfaces/MemberInfoDTO';
import { PlaylistService } from '../../services/playlist.service';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { VideoListDTO } from 'src/app/interfaces/VideoListDTO';
import { PlayListCreateDTO } from '../../interfaces/PlayListCreateDTO';

@Component({
  selector: 'app-playlistpost-aput',
  templateUrl: './playlistpost-aput.component.html',
  styleUrls: ['./playlistpost-aput.component.css'],
})
export class PlaylistpostAputComponent implements OnInit {
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
    private playlistService: PlaylistService
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
          this.playlistItems = items;
          this.selectedVideos = this.playlistItems.map((item) => ({
            videoId: item.videoId,
            videoName: item.videoName,
            episode: item.episode ?? 0,
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

  onSubmit(): void {
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
            this.updatePlaylistItems();

            console.log('圖片已編輯');
          },
          (error) => {}
        );
    } else {

      this.playlistService.addNewPlaylist(playListCreateDTO).subscribe(
        (response) => {
          const playListId = response.playListId;
          if (playListId) {
            this.playlistItems.forEach(
              (item) => (item.playListId = playListId)
            );
            this.playlistService
              .addPlaylistItems(playListId, this.playlistItems)
              .subscribe(
                (res) => {},
                (error) => {}
              );
          } else {
          }
        },
        (error) => {}
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
    moveItemInArray(
      this.playlistItems,
      event.previousIndex,
      event.currentIndex
    );

    this.playlistItems.forEach((video, index) => {
      console.log(
        `Updating position for videoId: ${video.videoId}, newPosition: ${
          index + 1
        }`
      );
      this.playlistService
        .updateVideoPosition(
          this.playlist.playListId ?? 0,
          video.videoId,
          index + 1
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
  }

  updateVideoPositions(): void {
    this.playlistItems.forEach((item, index) => {
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
