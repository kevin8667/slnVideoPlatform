import { Component, ChangeDetectorRef, OnInit } from '@angular/core';
import { PlaylistDTO } from '../../interfaces/PlaylistDTO';
import { PlaylistitemDTO } from '../../interfaces/PlaylistitemDTO';
import { MemberInfoDTO } from '../../interfaces/MemberInfoDTO';
import { PlaylistService } from '../../services/playlist.service';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { VideoListDTO } from 'src/app/interfaces/VideoListDTO';

@Component({
  selector: 'app-playlistpost-aput',
  templateUrl: './playlistpost-aput.component.html',
  styleUrls: ['./playlistpost-aput.component.css']
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
    showImage: null
  };

  playlistItems: PlaylistitemDTO[] = [];
  playlistCollaborators: MemberInfoDTO[] = [];
  availableCollaborators: MemberInfoDTO[] = [];
  allAvailableVideos: VideoListDTO[] = [];  // 儲存所有可用影片
  selectedVideos: VideoListDTO[] = [];   // 用於綁定 p-multiSelect 的已選擇影片

  imagePreview: string | ArrayBuffer | null = '/assets/img/noimageooo.jpg';

  constructor(
    private cdr: ChangeDetectorRef,
    private playlistService: PlaylistService
  ) {}

  ngOnInit(): void {
    this.loadAllAvailableVideos(); // 確保在組件初始化時調用
  }

  getImagePreview(): string {
    return this.imagePreview ? (this.imagePreview as string) : '/assets/img/noimageooo.jpg';
  }

  // 顯示對話框
  showDialog(isEditing: boolean, playlist?: PlaylistDTO): void {
    this.isEditing = isEditing;
    this.displayDialog = true;

    if (isEditing && playlist) {
      this.playlist = { ...playlist };
      this.imagePreview = this.playlist.showImage ? 'data:image/png;base64,' + this.playlist.showImage : '/assets/img/noimageooo.jpg';
      this.loadCollaborators();
      this.loadPlaylistItems();
    } else {
      this.resetForm();
      this.loadCollaborators();
      this.loadAllAvailableVideos();
    }
    this.cdr.detectChanges();
  }

  // 加載所有影片
  loadAllAvailableVideos(): void {
    this.playlistService.getAllVideos().subscribe(
      (videos: VideoListDTO[]) => {
        this.allAvailableVideos = videos; // 保持全部可選影片
      },
      (error) => {
        console.error('Error loading available videos', error);
      }
    );
  }

  // 加載播放清單中的影片（編輯模式）
  loadPlaylistItems(): void {
    this.playlistService.getPlaylistItems(this.playlist.playListId ?? 0).subscribe(
      (items: PlaylistitemDTO[]) => {
        this.playlistItems = items;  // 編輯模式顯示的卡片影片
        // 將播放清單項目映射到 VideoListDTO 格式，並自動勾選已選影片
        this.selectedVideos = this.playlistItems.map(item => ({
          videoId: item.videoId,
          videoName: item.videoName,
          episode: item.episode??0,
          thumbnailPath: item.thumbnailPath??''
        }));
      },
      (error) => {
        console.error('Error loading playlist items', error);
      }
    );
  }

  // 加載協作者
  loadCollaborators(): void {
    this.playlistService.getCollaborators().subscribe(
      (collaborators) => {
        this.availableCollaborators = collaborators;

        // 編輯模式下自動勾選協作者
        if (this.isEditing && this.playlist.playListId) {
          this.playlistService.getCollaborators(this.playlist.playListId).subscribe(
            (selectedCollaborators) => {
              this.playlistCollaborators = this.availableCollaborators.filter(collaborator =>
                selectedCollaborators.some(sc => sc.memberId === collaborator.memberId)
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

  // 當用戶選擇/取消選擇影片時，更新列表
  onVideoSelectionChange(event: any): void {
    this.selectedVideos = event.value; // 這將自動更新 `p-multiSelect` 的選擇
    // 更新playlistItems，確保所選影片顯示在卡片上
    this.selectedVideos.forEach(video => this.onSelectVideo(video));
  }

  // 重置表單（新增模式）
  resetForm(): void {
    this.playlist = {
      playListId: 0,
      playListName: '',
      playListDescription: '',
      viewCount: 100,
      likeCount: 100,
      addedCount: 100,
      sharedCount: 100,
      showImage: null
    };
    this.imagePreview = '/assets/img/noimageooo.jpg';
    this.playlistItems = [];
    this.playlistCollaborators = [];
    this.selectedVideos = [];
  }

  onCancel(): void {
    this.displayDialog = false;
  }

  // 保存播放清單
  onSubmit(): void {
    if (this.playlist.playListName.trim() === '' || this.playlist.playListDescription.trim() === '') {
      alert('請填寫所有必要的欄位');
      return;
    }

    const collaboratorIds = this.playlistCollaborators.map(c => c.memberId);

    const playlistDTO = {
      ...this.playlist,
      collaboratorIds: collaboratorIds
    };

    if (this.isEditing) {
      this.playlistService.editPlaylist(this.playlist.playListId ?? 0, playlistDTO).subscribe(
        (response) => {
          this.updatePlaylistItems();
        },
        (error) => {
          console.error('Error editing playlist', error);
        }
      );
    } else {
      this.playlistService.addNewPlaylist(playlistDTO).subscribe(
        (response) => {
          const playListId = response.playListId;
          this.playlistItems.forEach(item => item.playListId = playListId ?? 0);
          this.playlistService.addPlaylistItems(playListId ?? 0, this.playlistItems).subscribe(
            (res) => {
              console.log('影片已新增到播放清單:', res);
            },
            (error) => {
              console.error('Error adding videos to playlist', error);
            }
          );
        },
        (error) => {
          console.error('Error adding playlist', error);
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
        this.playlist.showImage = reader.result as string;
        this.cdr.detectChanges();
      };
    }
  }



  drop(event: CdkDragDrop<PlaylistitemDTO[]>): void {
    moveItemInArray(this.playlistItems, event.previousIndex, event.currentIndex);

    this.playlistItems.forEach((video, index) => {
      console.log(`Updating position for videoId: ${video.videoId}, newPosition: ${index + 1}`);
      this.playlistService.updateVideoPosition(this.playlist.playListId ?? 0, video.videoId, index + 1).subscribe(
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
      this.playlistService.updateVideoPosition(this.playlist.playListId ?? 0, item.videoId, index + 1).subscribe(
        (response) => {
          console.log('影片位置已更新', response);
        },
        (error) => {
          console.error('Error updating video position', error);
        }
      );
    });
  }

  // 更新播放清單中的影片項目
  updatePlaylistItems(): void {
    if (this.playlist.playListId) {
      this.playlistService.addPlaylistItems(this.playlist.playListId, this.playlistItems).subscribe(
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
    const existingVideo = this.playlistItems.find(video => video.videoId === item.videoId);

    // 如果影片已經存在於播放清單中，則不再添加
    if (!existingVideo) {
      this.playlistItems.push({
        playListId: this.playlist.playListId ?? 0,
        videoId: item.videoId,
        videoPosition: this.playlistItems.length + 1, // 確保視頻順序
        videoName: item.videoName,
        thumbnailPath: item.thumbnailPath,
        episode: item.episode ?? 0
      });
    }
  }

  removePlaylistItem(index: number): void {
    const video = this.selectedVideos[index];
    this.playlistService.removePlaylistItem(this.playlist.playListId ?? 0, video.videoId).subscribe(
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



