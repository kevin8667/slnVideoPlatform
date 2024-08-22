


function getSpots(data) {
    return data.forumResult.map(spot => createSpots(spot))
}

function createSpots(spot) {
    const { title, articleContent, memberName
        , themeName, articleId, lock, postDate, replyCount, updateDate } = spot
    return `<tr >
                <td>
                    ${themeName}
                </td>
                <td>
                    ${memberName}
                </td>
                <td>
                    ${title.length>20? title.substring(0,20)+'...':title.length}
                </td>
                <td>
                    <button type="button" class="btn btn-gradient me-1 mb-2 large-btn"
                                onclick="showDetails(${articleId})">
                        點擊顯示
                    </button>
                </td>
                <td>
                    ${postDate.replace('T',' ').substring(0, 16) }
                </td>
                <td>
                    ${replyCount}
                </td>
                <td>
                    ${lock === true ? '上架中' : ' X'}
                </td>
                <td>
                    <a href="/Articles/Edit/${articleId}" class="btn btn-gradient me-1 mb-2 large-btn">編輯</a>
                </td>
            </tr>
            `
}

function judgePages(data) {
    let strPageLi = ''
    let totalPages = data.totalPages
    let curretPages = forumDto.page
    //總頁超過十 選取頁低於總頁10就全部顯示
    if (totalPages <= 10) {
        strPageLi = allPages(1, totalPages)

    } else if (curretPages <= 3) { //超過10頁就用...分隔顯示最後一頁 且當前頁小於4就不顯示...分隔

        strPageLi += allPages(1, 8)
        strPageLi += `...
                                                        <li class="page-item"><a class="page-link" onclick="clickPages(${totalPages})">${totalPages}</a></li>`
    } else if (totalPages - curretPages >= 3) { //當前頁超過3
        strPageLi = `<li class="page-item"><a class="page-link" onclick="clickPages(1)">1</a></li>...`
        strPageLi += allPages(curretPages - 2, curretPages + 2)
        strPageLi += `...
                                                    <li class="page-item"><a class="page-link" onclick="clickPages(${totalPages})">${totalPages}</a></li>`
    } else {
        strPageLi = `<li class="page-item"><a class="page-link" onclick="clickPages(1)">1</a></li>...`
        strPageLi += allPages(totalPages - 4, totalPages)
    }

    liPages.innerHTML = strPageLi
    $('#h3').text(`共計有${data.totalCount}筆資料，現在是${curretPages}/${totalPages}頁`);

}

function allPages(start, end) {
    let PageLi = ''

    for (let i = start; i <= end; i++) {
        PageLi += `
        <li class="page-item"><a class="page-link" onclick="clickPages(${i})">${i}</a></li>
        `
    }
    return PageLi
}

function changePageSize(event) {
    forumDto.pageSize = event.target.value
    forumDto.page = 1
    load()
}

function inputKeyword(event) {


    forumDto.keyword = event.target.value
    forumDto.page = 1
    load()
}


const changeTheme = (categoryId, event) => {
    forumDto.page = 1
    forumDto.categoryId = categoryId
    load()
    document.getElementById('dropdownMenuButton').innerHTML = event.target.text
}

const clickPages = page => {
    forumDto.page = page
    load()
}

const changeSort = (event) => {
    const selectValue = event.target.value

    const sortType = selectValue.includes('asc') ? 'asc' : 'desc'
    forumDto.sortType = sortType

    const sortBy = selectValue.replace('asc', '')
    forumDto.sortBy = sortBy

    load()
}