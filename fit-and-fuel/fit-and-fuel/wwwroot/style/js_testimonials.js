class PTTestimonialsHandler extends elementorModules.frontend.handlers.Base {
  getDefaultSettings() {
    return {
      selectors: {
        swiper: '.swiper-container',
        prevEl: '.pt-swiper-button-prev',
        nextEl: '.pt-swiper-button-next',
        paginationEl: '.pt-swiper-pagination',
      }
    };
  }

  getDefaultElements() {
    const selectors = this.getSettings('selectors');

    return {
      $swiperContainer: this.$element.find(selectors.swiper),
      $prevEl: this.$element.find(selectors.prevEl),
      $nextEl: this.$element.find(selectors.nextEl),
      $paginationEl: this.$element.find(selectors.paginationEl),
    }
  }

  initSwiper() {
    const elementSettings = this.getElementSettings();
    const elements = this.getDefaultElements();
    const elementorBreakpoints = elementorFrontend.config.responsive.activeBreakpoints;

    if ('carousel' != elementSettings.layout) {
      return;
    }

    const swiperOptions = {
      slidesPerView: elementSettings.cols ? elementSettings.cols : 1,
      spaceBetween: 30,
      loop: 'yes' === elementSettings.loop,
      autoplay: 'yes' === elementSettings.autoplay,
      handleElementorBreakpoints: true,
    };

    swiperOptions.breakpoints = {};

    Object.keys(elementorBreakpoints).reverse().forEach((breakpointName) => {
      swiperOptions.breakpoints[elementorBreakpoints[breakpointName].value] = {
        slidesPerView: +elementSettings['cols_' + breakpointName]
      };
    });

    if ('yes' === elementSettings.arrows && 'style-1' === elementSettings.navigation_style) {
      swiperOptions.navigation = {
        nextEl: elements.$nextEl.get(0),
        prevEl: elements.$prevEl.get(0),
      };
    }

    if ('style-2' === elementSettings.navigation_style || 'style-3' === elementSettings.navigation_style) {
      swiperOptions.navigation = {
        nextEl: elements.$nextEl.get(0),
        prevEl: elements.$prevEl.get(0),
      };
    }

    if ('yes' === elementSettings.dots && 'style-1' === elementSettings.navigation_style) {
      swiperOptions.pagination = {
        el: elements.$paginationEl.get(0),
        type: 'bullets',
        clickable: true
      };
    }

    if ('yes' === elementSettings.fraction || 'style-2' === elementSettings.navigation_style || 'style-3' === elementSettings.navigation_style) {
      swiperOptions.pagination = {
        el: elements.$paginationEl.get(0),
        type: 'fraction'
      };
    }

    const Swiper = elementorFrontend.utils.swiper;

    this.swiper = new Swiper(elements.$swiperContainer.get(0), swiperOptions);
  }

  onInit() {
    this.initSwiper();
  }
}

jQuery(window).on('elementor/frontend/init', () => {
  elementorFrontend.elementsHandler.attachHandler('pt-testimonials', PTTestimonialsHandler);
});
