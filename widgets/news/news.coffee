class Dashing.News extends Dashing.Widget

  ready: ->
    @currentIndex = 0
    @headlineElem = $(@node).find('.headline-container')
    @nextComment()
    @startCarousel()

  onData: (data) ->
    @currentIndex = 0

  startCarousel: ->
    setInterval(@nextComment, 8000)

  nextComment: =>
    headlines = @get('headlines')
    if headlines
      @headlineElem.fadeOut =>
        @currentIndex = (@currentIndex + 1) % headlines.length
        @set 'current_headline', headlines[@currentIndex]
        @headlineElem.fadeIn()