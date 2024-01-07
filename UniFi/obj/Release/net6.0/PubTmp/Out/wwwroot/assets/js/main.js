(function ($) {
    "use strict";

    jQuery(document).ready(function ($) {



        $('.watch__big').slick({
            slidesToShow: 1,
            slidesToScroll: 1,
            arrows: true,
            prevArrow:"<button type='button' class='slick-prev pull-left'><i class='fa fa-angle-left' aria-hidden='true'></i></button>",
            nextArrow:"<button type='button' class='slick-next pull-right'><i class='fa fa-angle-right' aria-hidden='true'></i></button>",
            fade: true,
            asNavFor: '.watch__small'
        });
        $('.watch__small').slick({
            slidesToShow: 3,
            slidesToScroll: 1,
            asNavFor: '.watch__big',
            dots: false,
            arrows: false,
            centerMode: false,
            focusOnSelect: true
        });


        $('.feedback_active').slick({
            infinite: true,
            slidesToShow: 1,
            slidesToScroll: 1,
            prevArrow: '<img class="ar-lf" src="assets/img/arrow-lft.svg" alt="">',
            nextArrow: '<img class="ar-lf ar-ri" src="assets/img/arrow-right.svg" alt="">',
            dots: false,
            arrows: true,
        });


        $('.hero__active').slick({
            infinite: true,
            slidesToShow: 1,
            autoplay: true,
            autoplaySpeed: 2000,
            slidesToScroll: 1,
            dots: true,
            arrows: false,
        });








        $(".product__active").owlCarousel({
            items: 4,
            nav: true,
            dot: false,
            navText: ['<img src="assets/img/arrow-lft.svg" alt="">', '<img src="assets/img/arrow-right.svg" alt="">'],
            loop: true,
            margin: 20,
            autoplay: false,
            autoplayTimeout: 3000,
            smartSpeed: 1000,
            responsiveClass: true,
            responsive: {
                0: {
                    items: 2,

                },
                768: {
                    items: 3,

                },
                1000: {
                    items: 4,

                }
            }


        });



        $(".feedback__mobile__active").owlCarousel({
            items: 1,
            nav: false,
            dot: false,
            navText: ['<img src="assets/img/arrow-lft.svg" alt="">', '<img src="assets/img/arrow-right.svg" alt="">'],
            loop: true,
            stagePadding: 20,
            margin: 10,
            autoplay: false,
            autoplayTimeout: 3000,
            smartSpeed: 1000,
            responsiveClass: true,
            responsive: {
                0: {
                    items: 1,

                },
                768: {
                    items: 1,

                },
                1000: {
                    items: 1,

                }
            }


        });
    
        
        
        
        
        
        
        
        
        
        
        AOS.init();

// You can also pass an optional settings object
// below listed default settings
AOS.init({
  // Global settings:
  disable: false, // accepts following values: 'phone', 'tablet', 'mobile', boolean, expression or function
  startEvent: 'DOMContentLoaded', // name of the event dispatched on the document, that AOS should initialize on
  initClassName: 'aos-init', // class applied after initialization
  animatedClassName: 'aos-animate', // class applied on animation
  useClassNames: false, // if true, will add content of `data-aos` as classes on scroll
  disableMutationObserver: false, // disables automatic mutations' detections (advanced)
  debounceDelay: 50, // the delay on debounce used while resizing window (advanced)
  throttleDelay: 99, // the delay on throttle used while scrolling the page (advanced)
  

  // Settings that can be overridden on per-element basis, by `data-aos-*` attributes:
  offset: 120, // offset (in px) from the original trigger point
  delay: 0, // values from 0 to 3000, with step 50ms
  duration: 400, // values from 0 to 3000, with step 50ms
  easing: 'ease', // default easing for AOS animations
  once: true, // whether animation should happen only once - while scrolling down
  mirror: false, // whether elements should animate out while scrolling past them
  anchorPlacement: 'top-bottom', // defines which position of the element regarding to window should trigger the animation

});




    });


    jQuery(window).load(function () {


    });


}(jQuery));
